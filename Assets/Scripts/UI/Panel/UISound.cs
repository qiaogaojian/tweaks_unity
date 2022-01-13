using System;
using System.Collections;
using System.Collections.Generic;
using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UISound : BaseView
{
    private Button btnReturn;
    private Slider sliderGlobal;
    private Slider sliderMusic;
    private Slider sliderSound;
    private Button btnPause;
    private Button btnMultiSound;
    private Button btnStopSound;
    private Button btnPauseMusic;
    private Button btnStopMusic;
    private Button btnMusic;
    private Toggle toggleSound;
    private Toggle toggleMusic;
    private Button btnPlaySound1;
    private Button btnPlaySound2;
    private Button btnPlayMusic1;
    private Button btnPlayMusic2;

    private Transform soundLeft;
    private Transform soundRight;
    private Transform soundCircle;
    private int       circleCenterX = 0;
    private int       circleCenterY = 0;
    private int       circleRadius  = 300;
    private float     circleAngle   = 0;
    private Vector3   curPos;

    public override void InitView()
    {
        btnReturn     = transform.Find("btnReturn").GetComponent<Button>();

        sliderGlobal  = transform.Find("ivBg/sliderGlobal").GetComponent<Slider>();
        sliderMusic   = transform.Find("ivBg/sliderMusic").GetComponent<Slider>();
        sliderSound   = transform.Find("ivBg/sliderSound").GetComponent<Slider>();
        btnPause      = transform.Find("ivBg/btnPause").GetComponent<Button>();
        btnMultiSound = transform.Find("ivBg/btnMultiSound").GetComponent<Button>();
        btnStopSound  = transform.Find("ivBg/btnStopSound").GetComponent<Button>();
        btnPauseMusic = transform.Find("ivBg/btnPauseMusic").GetComponent<Button>();
        btnStopMusic  = transform.Find("ivBg/btnStopMusic").GetComponent<Button>();
        btnMusic      = transform.Find("ivBg/btnMusic").GetComponent<Button>();
        toggleSound   = transform.Find("ivBg/toggleSound").GetComponent<Toggle>();
        toggleMusic   = transform.Find("ivBg/toggleMusic").GetComponent<Toggle>();
        btnPlaySound1 = transform.Find("ivBg/btnSound1").GetComponent<Button>();
        btnPlaySound2 = transform.Find("ivBg/btnSound2").GetComponent<Button>();
        btnPlayMusic1 = transform.Find("ivBg/btnMusic1").GetComponent<Button>();
        btnPlayMusic2 = transform.Find("ivBg/btnMusic2").GetComponent<Button>();

        sliderGlobal.value = Framework.Sound.GlobalVolume;
        sliderMusic.value  = Framework.Sound.GlobalMusicVolume;
        sliderSound.value  = Framework.Sound.GlobalSoundsVolume;
        toggleSound.isOn   = Framework.Sound.GlobalSoundsStatus;
        toggleMusic.isOn   = Framework.Sound.GlobalMusicStatus;

        initSoundTransform();
    }

    private void initSoundTransform()
    {
        soundLeft                    = new GameObject("soundLeft").transform;
        soundLeft.transform.position = new Vector3(-300, 0, 0);

        soundRight                    = new GameObject("soundRight").transform;
        soundRight.transform.position = new Vector3(300, 0, 0);

        soundCircle = new GameObject("soundCircle").transform;
    }

    private void Update()
    {
        circleAngle                    += 0.01f;
        curPos.x                       =  (float) (circleRadius * Math.Cos(circleAngle));
        curPos.z                       =  (float) (circleRadius * Math.Sin(circleAngle));
        soundCircle.transform.position =  curPos;
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
        sliderGlobal.onValueChanged.AddListener(OnGlobalValueChanged);
        sliderMusic.onValueChanged.AddListener(OnMusicValueChanged);
        sliderSound.onValueChanged.AddListener(OnSoundValueChanged);
        btnPause.onClick.AddListener(OnClickBtnPause);
        btnMultiSound.onClick.AddListener(OnClickBtnMultiSound);
        btnStopSound.onClick.AddListener(OnClickBtnStopSound);
        btnPauseMusic.onClick.AddListener(OnClickBtnPauseMusic);
        btnStopMusic.onClick.AddListener(OnClickBtnStopMusic);
        btnMusic.onClick.AddListener(OnClickBtnMusic);
        toggleSound.onValueChanged.AddListener(OnClickToggleSound);
        toggleMusic.onValueChanged.AddListener(OnClickToggleMusic);
        btnPlaySound1.onClick.AddListener(OnClickBtnPlaySound1);
        btnPlaySound2.onClick.AddListener(OnClickBtnPlaySound2);
        btnPlayMusic1.onClick.AddListener(OnClickBtnPlayMusic1);
        btnPlayMusic2.onClick.AddListener(OnClickBtnPlayMusic2);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
        sliderGlobal.onValueChanged.RemoveListener(OnGlobalValueChanged);
        sliderMusic.onValueChanged.RemoveListener(OnMusicValueChanged);
        sliderSound.onValueChanged.RemoveListener(OnSoundValueChanged);
        btnPause.onClick.RemoveListener(OnClickBtnPause);
        btnMultiSound.onClick.RemoveListener(OnClickBtnMultiSound);
        btnStopSound.onClick.RemoveListener(OnClickBtnStopSound);
        btnPauseMusic.onClick.RemoveListener(OnClickBtnPauseMusic);
        btnStopMusic.onClick.RemoveListener(OnClickBtnStopMusic);
        btnMusic.onClick.RemoveListener(OnClickBtnMusic);
        toggleSound.onValueChanged.RemoveListener(OnClickToggleSound);
        toggleMusic.onValueChanged.RemoveListener(OnClickToggleMusic);
        btnPlaySound1.onClick.RemoveListener(OnClickBtnPlaySound1);
        btnPlaySound2.onClick.RemoveListener(OnClickBtnPlaySound2);
        btnPlayMusic1.onClick.RemoveListener(OnClickBtnPlayMusic1);
        btnPlayMusic2.onClick.RemoveListener(OnClickBtnPlayMusic2);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    private void OnGlobalValueChanged(float value)
    {
        Framework.Sound.GlobalVolume = value;
    }

    private void OnMusicValueChanged(float value)
    {
        Framework.Sound.GlobalMusicVolume = value;
    }

    private void OnSoundValueChanged(float value)
    {
        Framework.Sound.GlobalSoundsVolume = value;
    }

    private void OnClickBtnPause()
    {
        Framework.Sound.PauseAllSounds();
    }

    private void OnClickBtnMultiSound()
    {
        Framework.Sound.PlaySound(SoundType.Tick);
    }

    private void OnClickBtnStopSound()
    {
        Framework.Sound.StopAllSounds();
    }

    private void OnClickBtnPauseMusic()
    {
        Framework.Sound.PauseAllMusic();
    }

    private void OnClickBtnStopMusic()
    {
        Framework.Sound.StopAllMusic();
    }

    private void OnClickBtnMusic()
    {
        Framework.Sound.PlayMusic(MusicType.Thaw);
    }

    private void OnClickToggleSound(bool isOn)
    {
        Framework.Sound.GlobalSoundsStatus = isOn;
    }

    private void OnClickToggleMusic(bool isOn)
    {
        Framework.Sound.GlobalMusicStatus = isOn;
    }

    private void OnClickBtnPlaySound1()
    {
        int audioId = Framework.Sound.PlayEffect(SoundType.Tick, soundLeft);
        Framework.Sound.GetAudio(audioId).Max3DDistance = 1000;
        Framework.Sound.GetAudio(audioId).RolloffMode   = AudioRolloffMode.Linear;
    }

    private void OnClickBtnPlaySound2()
    {
        int audioId = Framework.Sound.PlayEffect(SoundType.Tock, soundRight);
        Framework.Sound.GetAudio(audioId).Max3DDistance = 1000;
        Framework.Sound.GetAudio(audioId).RolloffMode   = AudioRolloffMode.Linear;
    }

    private void OnClickBtnPlayMusic1()
    {
        int audioId = Framework.Sound.PlayBgm(MusicType.MondayCigarette, soundCircle);
        Framework.Sound.GetAudio(audioId).Max3DDistance = 1000;
        Framework.Sound.GetAudio(audioId).RolloffMode   = AudioRolloffMode.Linear;
    }

    private void OnClickBtnPlayMusic2()
    {
        int audioId = Framework.Sound.PlayBgm(MusicType.TheLastGoodbye, soundCircle);
        Framework.Sound.GetAudio(audioId).Max3DDistance = 1000;
        Framework.Sound.GetAudio(audioId).RolloffMode   = AudioRolloffMode.Linear;
    }
}