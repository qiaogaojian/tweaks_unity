//**********************************************************************
//#Author:  QiaoGaojian
//#Time:    2018/7/2
//**********************************************************************
//#Func:
//
// 声音管理器组件
//
// *. 无需安装,无需附着, SoundManager.CallFunction( ) 即可.
// *. 支持同时播放多个音效/音乐.
// *. 声音分为4类: music   sound    ui    bgm
// *. 声音的操作有4种:play/stop/pause/resume.  可以选择操作全部或个别.
// *. 循环音乐.
// *. 支持渐入渐出效果.
// *. 全局和个别音量控制.
// *. 音乐跨场景.
// *. 同时支持2D 3D声音.
//
// 声音管理器组件
//
// *. 将需要的音乐/音效名称 作为枚举SoundType MusicType
// *. 音乐音效文件存放在 StreamingAssets 文件夹中  结构为: Sounds/Bgm|Effect
// *. 不同平台打包时需要提供不同的声音格式:  .ogg(Android / Standalone)  .mp3(Android / IOS)
// *. 音效和音乐的开启和关闭在改变时会自动更新到Playerprefs中.
//
//**********************************************************************

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Mega
{
    public class SoundManager : GameComponent
    {
        /// <summary>
        /// 默认声音的位置
        /// </summary>
        public GameObject Gameobject
        {
            get { return gameObject; }
        }

        /// <summary>
        /// 单个音频是否允许多个声音
        /// </summary>
        public bool OnlyOneMusic { get; set; }

        /// <summary>
        /// 单个音频是否允许多个声音
        /// </summary>
        public bool OnlyOneSounds { get; set; }

        /// <summary>
        /// 单个音频是否允许多个声音
        /// </summary>
        public bool OnlyOneUISounds { get; set; }


        /// <summary>
        /// 当前音乐ID
        /// </summary>
        public MusicType CurMusicType
        {
            get { return curMusicType; }
            set { curMusicType = value; }
        }

        private MusicType curMusicType = MusicType.Max;

        /// <summary>
        /// 全局音量
        /// </summary>
        public float GlobalVolume
        {
            get { return GlobalConfig.GLOBALVOLUME; }
            set { GlobalConfig.GLOBALVOLUME = value; }
        }

        /// <summary>
        /// 音乐音量
        /// </summary>
        public float GlobalMusicVolume
        {
            get { return GlobalConfig.MUSICVOL; }
            set { GlobalConfig.MUSICVOL = value; }
        }

        /// <summary>
        /// 音效音量
        /// </summary>
        public float GlobalSoundsVolume
        {
            get { return GlobalConfig.SOUNDVOL; }
            set { GlobalConfig.SOUNDVOL = value; }
        }

        /// <summary>
        /// UI音量
        /// </summary>
        public float GlobalUISoundsVolume
        {
            get { return GlobalConfig.SOUNDVOL; }
            set { GlobalConfig.SOUNDVOL = value; }
        }

        /// <summary>
        /// BGM音量
        /// </summary>
        public float GlobalBgmVolume
        {
            get { return GlobalConfig.MUSICVOL; }
            set { GlobalConfig.MUSICVOL = value; }
        }

        /// <summary>
        /// 音效开关
        /// </summary>
        public bool GlobalSoundsStatus
        {
            get { return GlobalConfig.SOUNDSTATUS; }
            set { GlobalConfig.SOUNDSTATUS = value; }
        }

        /// <summary>
        /// 音乐开关
        /// </summary>
        public bool GlobalMusicStatus
        {
            get { return GlobalConfig.MUSICSTATUS; }
            set { GlobalConfig.MUSICSTATUS = value; }
        }

        /// <summary>
        /// 音效资源缓存
        /// </summary>
        private Dictionary<string, AudioClip> AudioDic = new Dictionary<string, AudioClip>();

        /// <summary>
        /// 音乐声音缓存
        /// </summary>
        private Dictionary<int, Audio> musicAudio = new Dictionary<int, Audio>();

        /// <summary>
        /// 音效声音缓存
        /// </summary>
        private Dictionary<int, Audio> soundsAudio = new Dictionary<int, Audio>();

        /// <summary>
        /// UI音效声音缓存
        /// </summary>
        private Dictionary<int, Audio> UISoundsAudio = new Dictionary<int, Audio>();

        /// <summary>
        /// Bgm音乐缓存
        /// </summary>
        private Dictionary<int, Audio> bgmAudio = new Dictionary<int, Audio>();

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool initialized = false;

        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            if (!initialized)
            {
                GlobalVolume         = GlobalConfig.GLOBALVOLUME;
                GlobalSoundsVolume   = GlobalConfig.SOUNDSTATUS ? GlobalConfig.SOUNDVOL : 0;
                GlobalUISoundsVolume = GlobalConfig.SOUNDSTATUS ? GlobalConfig.SOUNDVOL : 0;
                GlobalMusicVolume    = GlobalConfig.MUSICSTATUS ? GlobalConfig.MUSICVOL : 0;
                GlobalBgmVolume      = GlobalConfig.MUSICSTATUS ? GlobalConfig.MUSICVOL : 0;

                OnlyOneMusic    = false;
                OnlyOneSounds   = false;
                OnlyOneUISounds = false;

                initialized = true;
            }
        }

        private  Action OnloadFinish;
        private int allCount;
        private int curCount = 2; // 两个无效枚举 Max

        public void LoadAllAudio(Action onLoadFinish)
        {
            this.OnloadFinish = onLoadFinish;
            allCount = Enum.GetValues(typeof(SoundType)).Length + Enum.GetValues(typeof(MusicType)).Length;
            
            foreach (SoundType sound in Enum.GetValues(typeof(SoundType)))
            {
                if (sound != SoundType.Max)
                {
                    StartCoroutine(LoadAudioClip("Effect/", sound.ToString()));
                }
            }

            foreach (MusicType sound in Enum.GetValues(typeof(MusicType)))
            {
                if (sound != MusicType.Max)
                {
                    StartCoroutine(LoadAudioClip("Bgm/", sound.ToString()));
                }
            }
        }

        IEnumerator LoadAudioClip(string prePath, string clipName)
        {
            string path = FileUtils.GetRealUri("Sounds/" + prePath + clipName + ".OGG", PathMode.Streaming);

            using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.AUDIOQUEUE))
            {
                yield return webRequest.SendWebRequest();
                curCount++;
                if (curCount>=allCount)
                {
                    OnloadFinish.Invoke();
                }
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(webRequest);
                    AudioDic[clipName] = clip;
                    Debug.Log("set audio clip" + clipName);
                }
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// 场景加载回调
        /// </summary>
        /// <param name="scene">The scene that is loaded</param>
        /// <param name="mode">The scene load mode</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Stop and remove all non-persistent audio
            RemoveNonPersistAudio(musicAudio);
            RemoveNonPersistAudio(soundsAudio);
            RemoveNonPersistAudio(UISoundsAudio);
            RemoveNonPersistAudio(bgmAudio);
        }

        private void Update()
        {
            UpdateAllAudio(musicAudio);
            UpdateAllAudio(soundsAudio);
            UpdateAllAudio(UISoundsAudio);
            UpdateAllAudio(bgmAudio);
        }

        /// <summary>
        /// 获取声音缓存
        /// </summary>
        /// <param name="audioType">The audio type of the dictionary to return</param>
        /// <returns>An audio dictionary</returns>
        private Dictionary<int, Audio> GetAudioTypeDictionary(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = new Dictionary<int, Audio>();
            switch (audioType)
            {
                case Audio.AudioType.Music:
                    audioDict = musicAudio;
                    break;
                case Audio.AudioType.Sound:
                    audioDict = soundsAudio;
                    break;
                case Audio.AudioType.UISound:
                    audioDict = UISoundsAudio;
                    break;
                case Audio.AudioType.BGM:
                    audioDict = bgmAudio;
                    break;
            }

            return audioDict;
        }

        /// <summary>
        /// 获取多声音设置
        /// </summary>
        /// <param name="audioType">The audio type that the returned IgnoreDuplicates setting affects</param>
        /// <returns>An IgnoreDuplicates setting (bool)</returns>
        private bool GetAudioTypeIgnoreDuplicateSetting(Audio.AudioType audioType)
        {
            switch (audioType)
            {
                case Audio.AudioType.Music:
                    return OnlyOneMusic;
                case Audio.AudioType.Sound:
                    return OnlyOneSounds;
                case Audio.AudioType.UISound:
                    return OnlyOneUISounds;
                case Audio.AudioType.BGM:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 更新声音状态
        /// </summary>
        /// <param name="audioDict">The audio dictionary to update</param>
        private void UpdateAllAudio(Dictionary<int, Audio> audioDict)
        {
            // Go through all audios and update them
            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                audio.Update();

                // Remove it if it is no longer active (playing)
                if (!audio.IsPlaying && !audio.Paused)
                {
                    Destroy(audio.AudioSource);
                    audioDict.Remove(key);
                }
            }
        }

        /// <summary>
        /// 移除所有非持久声音
        /// </summary>
        /// <param name="audioDict">The audio dictionary whose non-persistant audios are getting removed</param>
        private void RemoveNonPersistAudio(Dictionary<int, Audio> audioDict)
        {
            // Go through all audios and remove them if they should not persist through scenes
            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                if (!audio.Persist && audio.Activated)
                {
                    Destroy(audio.AudioSource);
                    audioDict.Remove(key);
                }
            }
        }

        #region GetAudio Functions

        /// <summary>
        /// 获取缓存声音
        /// </summary>
        /// <param name="audioID">The id of the Audio to be retrieved</param>
        /// <returns>Audio that has as its id the audioID, null if no such Audio is found</returns>
        public Audio GetAudio(int audioID)
        {
            Audio audio;

            audio = GetMusicAudio(audioID);
            if (audio != null)
            {
                return audio;
            }

            audio = GetSoundAudio(audioID);
            if (audio != null)
            {
                return audio;
            }

            audio = GetUISoundAudio(audioID);
            if (audio != null)
            {
                return audio;
            }

            audio = GetBgmAudio(audioID);
            if (audio != null)
            {
                return audio;
            }

            return null;
        }

        /// <summary>
        /// 获取缓存声音
        /// </summary>
        /// <param name="audioClip">The audio clip of the Audio to be retrieved</param>
        /// <returns>First occurrence of Audio that has as plays the audioClip, null if no such Audio is found</returns>
        public Audio GetAudio(AudioClip audioClip)
        {
            Audio audio = GetMusicAudio(audioClip);
            if (audio != null)
            {
                return audio;
            }

            audio = GetSoundAudio(audioClip);
            if (audio != null)
            {
                return audio;
            }

            audio = GetUISoundAudio(audioClip);
            if (audio != null)
            {
                return audio;
            }

            audio = GetBgmAudio(audioClip);
            if (audio != null)
            {
                return audio;
            }

            return null;
        }

        /// <summary>
        /// Returns the music Audio that has as its id the audioID if one is found, returns null if no such Audio is found
        /// </summary>
        /// <param name="audioID">The id of the music Audio to be returned</param>
        /// <returns>Music Audio that has as its id the audioID if one is found, null if no such Audio is found</returns>
        public Audio GetMusicAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.Music, audioID);
        }

        /// <summary>
        /// Returns the first occurrence of music Audio that plays the given audioClip. Returns null if no such Audio is found
        /// </summary>
        /// <param name="audioClip">The audio clip of the music Audio to be retrieved</param>
        /// <returns>First occurrence of music Audio that has as plays the audioClip, null if no such Audio is found</returns>
        public Audio GetMusicAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.Music, audioClip);
        }

        /// <summary>
        /// Returns the sound fx Audio that has as its id the audioID if one is found, returns null if no such Audio is found
        /// </summary>
        /// <param name="audioID">The id of the sound fx Audio to be returned</param>
        /// <returns>Sound fx Audio that has as its id the audioID if one is found, null if no such Audio is found</returns>
        public Audio GetSoundAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.Sound, audioID);
        }

        /// <summary>
        /// Returns the first occurrence of sound Audio that plays the given audioClip. Returns null if no such Audio is found
        /// </summary>
        /// <param name="audioClip">The audio clip of the sound Audio to be retrieved</param>
        /// <returns>First occurrence of sound Audio that has as plays the audioClip, null if no such Audio is found</returns>
        public Audio GetSoundAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.Sound, audioClip);
        }

        /// <summary>
        /// Returns the UI sound fx Audio that has as its id the audioID if one is found, returns null if no such Audio is found
        /// </summary>
        /// <param name="audioID">The id of the UI sound fx Audio to be returned</param>
        /// <returns>UI sound fx Audio that has as its id the audioID if one is found, null if no such Audio is found</returns>
        public Audio GetUISoundAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.UISound, audioID);
        }

        /// <summary>
        /// Returns the first occurrence of UI sound Audio that plays the given audioClip. Returns null if no such Audio is found
        /// </summary>
        /// <param name="audioClip">The audio clip of the UI sound Audio to be retrieved</param>
        /// <returns>First occurrence of UI sound Audio that has as plays the audioClip, null if no such Audio is found</returns>
        public Audio GetUISoundAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.UISound, audioClip);
        }

        /// <summary>
        /// Returns the UI sound fx Audio that has as its id the audioID if one is found, returns null if no such Audio is found
        /// </summary>
        /// <param name="audioID">The id of the UI sound fx Audio to be returned</param>
        /// <returns>UI sound fx Audio that has as its id the audioID if one is found, null if no such Audio is found</returns>
        public Audio GetBgmAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.BGM, audioID);
        }

        /// <summary>
        /// Returns the first occurrence of UI sound Audio that plays the given audioClip. Returns null if no such Audio is found
        /// </summary>
        /// <param name="audioClip">The audio clip of the UI sound Audio to be retrieved</param>
        /// <returns>First occurrence of UI sound Audio that has as plays the audioClip, null if no such Audio is found</returns>
        public Audio GetBgmAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.BGM, audioClip);
        }

        private Audio GetAudio(Audio.AudioType audioType, int audioID)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            if (audioDict.ContainsKey(audioID))
            {
                return audioDict[audioID];
            }

            return null;
        }

        private Audio GetAudio(Audio.AudioType audioType, AudioClip audioClip)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> audioTypeKeys = new List<int>(audioDict.Keys);
            List<int> keys          = audioTypeKeys;
            foreach (int key in keys)
            {
                Audio audio = null;
                if (audioDict.ContainsKey(key))
                {
                    audio = audioDict[key];
                }

                if (audio == null)
                {
                    return null;
                }

                if (audio.Clip == audioClip && audio.Type == audioType)
                {
                    return audio;
                }
            }

            return null;
        }

        #endregion

        #region Get Audio Clip

        /// <summary>
        /// 根据 声音枚举 获取Audioclip
        /// </summary>
        /// <returns>The sound audio clip.</returns>
        /// <param name="soundType">Sound type.</param>
        public AudioClip GetSoundAudioClip(SoundType soundType)
        {
            if (AudioDic.ContainsKey(soundType.ToString()))
            {
                return AudioDic[soundType.ToString()];
            }


            string filePath = FileUtils.GetRealUri("Sounds/Effect/" + soundType + ".OGG", PathMode.Streaming);

            using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.OGGVORBIS))
            {
                request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    Debuger.Log(request.error);
                    return null;
                }

                while (!request.isDone)
                {
                    Debuger.Log("Loading Sound" + (request.downloadProgress * 100) + " %");
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                AudioDic[soundType.ToString()] = clip;
                return clip;
            }
        }

        /// <summary>
        /// 根据声音枚举和随机个数 随机获取AudioClip
        /// </summary>
        /// <returns>The random sound audio clip.</returns>
        /// <param name="soundType">声音枚举.</param>
        /// <param name="range">随机个数.</param>
        public AudioClip GetRandomSoundAudioClip(SoundType soundType, int range)
        {
            return GetSoundAudioClip((SoundType) (soundType + Random.Range(0, range)));
        }

        /// <summary>
        /// 根据 声音枚举 获取Audioclip
        /// </summary>
        /// <returns>The sound audio clip.</returns>
        /// <param name="soundType">Sound type.</param>
        public AudioClip GetMusicAudioClip(MusicType musicType)
        {

            if (AudioDic.ContainsKey(musicType.ToString()))
            {
                return AudioDic[musicType.ToString()];
            }

            string filePath = FileUtils.GetRealUri("Sounds/Bgm/" + musicType + ".OGG", PathMode.Streaming);

            using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.OGGVORBIS))
            {
                request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    Debuger.Log(request.error);
                    return null;
                }

                while (!request.isDone)
                {
                    Debuger.Log("Loading Music" + (request.downloadProgress * 100) + " %");
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                AudioDic[musicType.ToString()] = clip;
                return clip;
            }
        }

        /// <summary>
        /// 根据声音枚举和随机个数 随机获取AudioClip
        /// </summary>
        /// <returns>The random sound audio clip.</returns>
        /// <param name="soundType">声音枚举.</param>
        /// <param name="range">随机个数.</param>
        public AudioClip GetRandomMusicAudioClip(MusicType musicType, int range)
        {
            return GetMusicAudioClip((MusicType) (musicType + Random.Range(0, range)));
        }

        #endregion

        #region Prepare Function

        /// <summary>
        /// Prepares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, 1f, false, false, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip, float volume)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, volume, false, false, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name = "persist" > Whether the audio persists in between scene changes</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip, float volume, bool loop, bool persist)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, volume, loop, persist, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Prerpares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name="persist"> Whether the audio persists in between scene changes</param>
        /// <param name="fadeInValue">How many seconds it needs for the audio to fade in/ reach target volume (if higher than current)</param>
        /// <param name="fadeOutValue"> How many seconds it needs for the audio to fade out/ reach target volume (if lower than current)</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes background music
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name="persist"> Whether the audio persists in between scene changes</param>
        /// <param name="fadeInValue">How many seconds it needs for the audio to fade in/ reach target volume (if higher than current)</param>
        /// <param name="fadeOutValue"> How many seconds it needs for the audio to fade out/ reach target volume (if lower than current)</param>
        /// <param name="currentMusicfadeOutSeconds"> How many seconds it needs for current music audio to fade out. It will override its own fade out seconds. If -1 is passed, current music will keep its own fade out seconds</param>
        /// <param name="sourceTransform">The transform that is the source of the music (will become 3D audio). If 3D audio is not wanted, use null</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            return PrepareAudio(Audio.AudioType.Music, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        /// <summary>
        /// Prepares and initializes a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareSound(AudioClip clip)
        {
            return PrepareAudio(Audio.AudioType.Sound, clip, 1f, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareSound(AudioClip clip, float volume)
        {
            return PrepareAudio(Audio.AudioType.Sound, clip, volume, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="loop">Wether the sound is looped</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareSound(AudioClip clip, bool loop)
        {
            return PrepareAudio(Audio.AudioType.Sound, clip, 1f, loop, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes a sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the sound is looped</param>
        /// <param name="sourceTransform">The transform that is the source of the sound (will become 3D audio). If 3D audio is not wanted, use null</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareSound(AudioClip clip, float volume, bool loop, Transform sourceTransform)
        {
            return PrepareAudio(Audio.AudioType.Sound, clip, volume, loop, false, 0f, 0f, -1f, sourceTransform);
        }

        /// <summary>
        /// Prepares and initializes a UI sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareUISound(AudioClip clip)
        {
            return PrepareAudio(Audio.AudioType.UISound, clip, 1f, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Prepares and initializes a UI sound fx
        /// </summary>
        /// <param name="clip">The audio clip to prepare</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PrepareUISound(AudioClip clip, float volume)
        {
            return PrepareAudio(Audio.AudioType.UISound, clip, volume, false, false, 0f, 0f, -1f, null);
        }

        private int PrepareAudio(Audio.AudioType audioType, AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            if (clip == null)
            {
                Debuger.LogError("[Eazy Sound Manager] Audio clip is null", clip);
            }

            Dictionary<int, Audio> audioDict            = GetAudioTypeDictionary(audioType);
            bool                   ignoreDuplicateAudio = GetAudioTypeIgnoreDuplicateSetting(audioType);

            if (ignoreDuplicateAudio)
            {
                Audio duplicateAudio = GetAudio(audioType, clip);
                if (duplicateAudio != null)
                {
                    return duplicateAudio.AudioID;
                }
            }

            // Create the audioSource
            Audio audio = new Audio(audioType, clip, loop, persist, volume, fadeInSeconds, fadeOutSeconds, sourceTransform);

            // Add it to dictionary
            audioDict.Add(audio.AudioID, audio);

            return audio.AudioID;
        }

        #endregion

        #region Play Functions

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayMusic(MusicType clip)
        {
            return PlayMusic(Audio.AudioType.Music, clip, 1f, false, false, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayMusic(MusicType clip, Transform transform)
        {
            return PlayMusic(Audio.AudioType.Music, clip, 1f, false, false, 1f, 1f, -1f, transform);
        }

        /// <summary>
        /// 播放背景音乐,同一个音频文件同一时间只能播放一个
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayMusicOne(MusicType clip)
        {
            OnlyOneMusic = true;
            int audioId = PlayMusic(Audio.AudioType.Music, clip, 1f, false, false, 1f, 1f, -1f, null);
            OnlyOneMusic = false;
            return audioId;
        }

        /// <summary>
        /// 播放背景音乐,同一个音频文件同一时间只能播放一个
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayMusicOne(MusicType clip, Transform transform)
        {
            OnlyOneMusic = true;
            int audioId = PlayMusic(Audio.AudioType.Music, clip, 1f, false, false, 1f, 1f, -1f, transform);
            OnlyOneMusic = false;
            return audioId;
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayMusic(MusicType clip, float volume)
        {
            return PlayMusic(Audio.AudioType.Music, clip, volume, false, false, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name = "persist" > Whether the audio persists in between scene changes</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayMusic(MusicType clip, float volume, bool loop, bool persist)
        {
            return PlayMusic(Audio.AudioType.Music, clip, volume, loop, persist, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name="persist"> Whether the audio persists in between scene changes</param>
        /// <param name="fadeInSeconds">How many seconds it needs for the audio to fade in/ reach target volume (if higher than current)</param>
        /// <param name="fadeOutSeconds"> How many seconds it needs for the audio to fade out/ reach target volume (if lower than current)</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayMusic(MusicType clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds)
        {
            return PlayMusic(Audio.AudioType.Music, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, -1f, null);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the music is looped</param>
        /// <param name="persist"> Whether the audio persists in between scene changes</param>
        /// <param name="fadeInSeconds">How many seconds it needs for the audio to fade in/ reach target volume (if higher than current)</param>
        /// <param name="fadeOutSeconds"> How many seconds it needs for the audio to fade out/ reach target volume (if lower than current)</param>
        /// <param name="currentMusicfadeOutSeconds"> How many seconds it needs for current music audio to fade out. It will override its own fade out seconds. If -1 is passed, current music will keep its own fade out seconds</param>
        /// <param name="sourceTransform">The transform that is the source of the music (will become 3D audio). If 3D audio is not wanted, use null</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayMusic(MusicType clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            return PlayMusic(Audio.AudioType.Music, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlaySound(SoundType clip)
        {
            return PlaySound(Audio.AudioType.Sound, clip, 1f, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlaySound(SoundType clip, Transform sourceTransform)
        {
            return PlaySound(clip, 1, false, sourceTransform);
        }


        /// <summary>
        /// 播放音效,同一个音频文件同一时间只能播放一个
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayEffect(SoundType clip)
        {
            OnlyOneSounds = true;
            int AudioId = PlaySound(Audio.AudioType.Sound, clip, 1f, false, false, 0f, 0f, -1f, null);
            OnlyOneSounds = false;
            return AudioId;
        }

        /// <summary>
        /// 播放音效,同一个音频文件同一时间只能播放一个
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayEffect(SoundType clip, Transform sourceTransform)
        {
            OnlyOneSounds = true;
            int AudioId = PlaySound(clip, 1, false, sourceTransform);
            OnlyOneSounds = false;
            return AudioId;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlaySound(SoundType clip, float volume)
        {
            return PlaySound(Audio.AudioType.Sound, clip, volume, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="loop">Wether the sound is looped</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlaySound(SoundType clip, bool loop)
        {
            return PlaySound(Audio.AudioType.Sound, clip, 1f, loop, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <param name="loop">Wether the sound is looped</param>
        /// <param name="sourceTransform">The transform that is the source of the sound (will become 3D audio). If 3D audio is not wanted, use null</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlaySound(SoundType clip, float volume, bool loop, Transform sourceTransform)
        {
            return PlaySound(Audio.AudioType.Sound, clip, volume, loop, false, 0f, 0f, -1f, sourceTransform);
        }

        /// <summary>
        /// Play a UI sound fx
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayUISound(SoundType clip)
        {
            return PlaySound(Audio.AudioType.UISound, clip, 1f, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Play a UI sound fx
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayUISound(SoundType clip, float volume)
        {
            return PlaySound(Audio.AudioType.UISound, clip, volume, false, false, 0f, 0f, -1f, null);
        }

        /// <summary>
        /// Play a Bgm
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayBgm(MusicType clip)
        {
            StopAllBgm();
            return PlayMusic(Audio.AudioType.BGM, clip, 1f, true, true, 1f, 1f, -1f, null);
        }

        /// <summary>
        /// Play a Bgm
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayBgm(MusicType clip, Transform transform)
        {
            StopAllBgm();
            return PlayMusic(Audio.AudioType.BGM, clip, 1f, true, true, 1f, 1f, -1f, transform);
        }

        /// <summary>
        /// Play a UI sound fx
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume"> The volume the music will have</param>
        /// <returns>The ID of the created Audio object</returns>
        public int PlayBgm(MusicType clip, float volume)
        {
            StopAllBgm();
            return PlayMusic(Audio.AudioType.BGM, clip, volume, true, true, 1f, 1f, -1f, null);
        }

        public int PlayMusic(Audio.AudioType audioType, MusicType musicType, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            AudioClip clip = GetMusicAudioClip(musicType);

            if (clip == null)
            {
                Debuger.LogError("Sound Manager: Audio clip is null, cannot play music", clip);
                return -1;
            }

            return PlayAudio(audioType, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        public int PlaySound(Audio.AudioType audioType, SoundType soundType, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            AudioClip clip = GetSoundAudioClip(soundType);

            if (clip == null)
            {
                Debuger.LogError("Sound Manager: Audio clip is null, cannot play sound", clip);
                return -1;
            }

            return PlayAudio(audioType, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        private int PlayAudio(Audio.AudioType audioType, AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            int audioID = PrepareAudio(audioType, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
            GetAudio(audioType, audioID).Play();
            return audioID;
        }

        #endregion

        #region Stop Functions

        /// <summary>
        /// Stop all audio playing
        /// </summary>
        public void StopAll()
        {
            StopAll(-1f);
        }

        /// <summary>
        /// Stop all audio playing
        /// </summary>
        /// <param name="musicFadeOutSeconds"> How many seconds it needs for all music audio to fade out. It will override  their own fade out seconds. If -1 is passed, all music will keep their own fade out seconds</param>
        public void StopAll(float musicFadeOutSeconds)
        {
            StopAllMusic(musicFadeOutSeconds);
            StopAllSounds();
            StopAllUISounds();
            StopAllBgm();
        }

        /// <summary>
        /// Stop all music playing
        /// </summary>
        public void StopAllMusic()
        {
            StopAllBgm();
            StopAllAudio(Audio.AudioType.Music, -1f);
        }

        /// <summary>
        /// Stop all music playing
        /// </summary>
        /// <param name="fadeOutSeconds"> How many seconds it needs for all music audio to fade out. It will override  their own fade out seconds. If -1 is passed, all music will keep their own fade out seconds</param>
        public void StopAllMusic(float fadeOutSeconds)
        {
            StopAllBgm();
            StopAllAudio(Audio.AudioType.Music, fadeOutSeconds);
        }

        /// <summary>
        /// Stop all sound fx playing
        /// </summary>
        public void StopAllSounds()
        {
            StopAllUISounds();
            StopAllAudio(Audio.AudioType.Sound, -1f);
        }

        /// <summary>
        /// Stop all UI sound fx playing
        /// </summary>
        public void StopAllUISounds()
        {
            StopAllAudio(Audio.AudioType.UISound, -1f);
        }

        /// <summary>
        /// Stop all UI sound fx playing
        /// </summary>
        public void StopAllBgm()
        {
            StopAllAudio(Audio.AudioType.BGM, -1f);
        }

        private void StopAllAudio(Audio.AudioType audioType, float fadeOutSeconds)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                if (fadeOutSeconds > 0)
                {
                    audio.FadeOutSeconds = fadeOutSeconds;
                }

                audio.Stop();
            }
        }

        #endregion

        #region Pause Functions

        /// <summary>
        /// Pause all audio playing
        /// </summary>
        public void PauseAll()
        {
            PauseAllMusic();
            PauseAllSounds();
            PauseAllUISounds();
            PauseAllBgm();
        }

        /// <summary>
        /// Pause all music playing
        /// </summary>
        public void PauseAllMusic()
        {
            PauseAllBgm();
            PauseAllAudio(Audio.AudioType.Music);
        }

        /// <summary>
        /// Pause all sound fx playing
        /// </summary>
        public void PauseAllSounds()
        {
            PauseAllUISounds();
            PauseAllAudio(Audio.AudioType.Sound);
        }

        /// <summary>
        /// Pause all UI sound fx playing
        /// </summary>
        public void PauseAllUISounds()
        {
            PauseAllAudio(Audio.AudioType.UISound);
        }

        /// <summary>
        /// Pause all UI sound fx playing
        /// </summary>
        public void PauseAllBgm()
        {
            PauseAllAudio(Audio.AudioType.BGM);
        }

        private void PauseAllAudio(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                audio.Pause();
            }
        }

        #endregion

        #region Resume Functions

        /// <summary>
        /// Resume all audio playing
        /// </summary>
        public void ResumeAll()
        {
            ResumeAllMusic();
            ResumeAllSounds();
            ResumeAllUISounds();
            ResumeAllBgm();
        }

        /// <summary>
        /// Resume all music playing
        /// </summary>
        public void ResumeAllMusic()
        {
            ResumeAllBgm();
            ResumeAllAudio(Audio.AudioType.Music);
        }

        /// <summary>
        /// Resume all sound fx playing
        /// </summary>
        public void ResumeAllSounds()
        {
            ResumeAllUISounds();
            ResumeAllAudio(Audio.AudioType.Sound);
        }

        /// <summary>
        /// Resume all UI sound fx playing
        /// </summary>
        public void ResumeAllUISounds()
        {
            ResumeAllAudio(Audio.AudioType.UISound);
        }

        public void ResumeAllBgm()
        {
            ResumeAllAudio(Audio.AudioType.BGM);
        }

        private void ResumeAllAudio(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                audio.Resume();
            }
        }

        #endregion
    }
}