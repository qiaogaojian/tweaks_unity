//**********************************************************************
//#Author:	Michael
//#Time:	2018/7/30
//**********************************************************************
//#Func:	全局配置信息,存储的都是需要固化的数据,采用PlayerPrefs存储
//**********************************************************************

using UnityEngine;

namespace Mega
{
    public static class GlobalConfig
    {
        #region Field

        //声音设置
        private static bool  soundStatus  = true;
        private static bool  musicStatus  = true;
        private static float globalVolume = 1f;
        private static float musicVol     = 1f;
        private static float soundVol     = 1f;
        private static float uiSoundVol   = 1f;

        //游戏资源路径模式
        private static PathMode pathMode = PathMode.Streaming;

        #endregion

        #region Property

        public static PathMode PATHMODE
        {
            get
            {
                if (PlayerPrefs.HasKey("pathMode"))
                {
                    pathMode = (PathMode) PlayerPrefs.GetInt("pathMode");
                }
                else
                {
                    PlayerPrefs.SetInt("pathMode", (int) pathMode);
                }

                return pathMode;
            }
            set
            {
                pathMode = value;
                PlayerPrefs.SetInt("pathMode", (int) pathMode);
            }
        }

        public static float UISOUNDVOL
        {
            get
            {
                if (PlayerPrefs.HasKey("uiSoundVol"))
                {
                    uiSoundVol = soundStatus ? PlayerPrefs.GetFloat("uiSoundVol") : 0;
                }
                else
                {
                    PlayerPrefs.SetFloat("uiSoundVol", uiSoundVol);
                }

                return uiSoundVol;
            }
            set
            {
                uiSoundVol = value;
                PlayerPrefs.SetFloat("uiSoundVol", uiSoundVol);
                uiSoundVol = soundStatus ? uiSoundVol : 0;
            }
        }

        public static float SOUNDVOL
        {
            get
            {
                if (PlayerPrefs.HasKey("soundVol"))
                {
                    soundVol = soundStatus ? PlayerPrefs.GetFloat("soundVol") : 0;
                }
                else
                {
                    PlayerPrefs.SetFloat("soundVol", soundVol);
                }

                return soundVol;
            }
            set
            {
                soundVol = value;
                PlayerPrefs.SetFloat("soundVol", soundVol);
                soundVol = soundStatus ? soundVol : 0;
            }
        }

        public static float MUSICVOL
        {
            get
            {
                if (PlayerPrefs.HasKey("musicVol"))
                {
                    musicVol = musicStatus ? PlayerPrefs.GetFloat("musicVol") : 0;
                }
                else
                {
                    PlayerPrefs.SetFloat("musicVol", musicVol);
                }

                return musicVol;
            }
            set
            {
                musicVol = value;
                PlayerPrefs.SetFloat("musicVol", musicVol);
                musicVol = musicStatus ? musicVol : 0;
            }
        }

        public static float GLOBALVOLUME
        {
            get
            {
                if (PlayerPrefs.HasKey("globalVolume"))
                {
                    globalVolume = PlayerPrefs.GetFloat("globalVolume");
                }
                else
                {
                    PlayerPrefs.SetFloat("globalVolume", globalVolume);
                }

                return globalVolume;
            }
            set
            {
                globalVolume = value;
                PlayerPrefs.SetFloat("globalVolume", globalVolume);
            }
        }

        public static bool SOUNDSTATUS
        {
            get
            {
                if (PlayerPrefs.HasKey("soundStatus"))
                {
                    soundStatus = PlayerPrefs.GetInt("soundStatus") == 1 ? true : false;
                }
                else
                {
                    PlayerPrefs.SetInt("soundStatus", soundStatus ? 1 : 0);
                }

                return soundStatus;
            }
            set
            {
                soundStatus = value;
                if (soundStatus)
                {
                    PlayerPrefs.SetInt("soundStatus", 1);
                    SOUNDVOL   = 1f;
                    UISOUNDVOL = 1f;
                }
                else
                {
                    PlayerPrefs.SetInt("soundStatus", 0);
                    SOUNDVOL   = 0;
                    UISOUNDVOL = 0;
                }
            }
        }

        public static bool MUSICSTATUS
        {
            get
            {
                if (PlayerPrefs.HasKey("musicStatus"))
                {
                    musicStatus = PlayerPrefs.GetInt("musicStatus") == 1 ? true : false;
                }
                else
                {
                    PlayerPrefs.SetInt("musicStatus", musicStatus ? 1 : 0);
                }

                return musicStatus;
            }
            set
            {
                musicStatus = value;
                if (musicStatus)
                {
                    PlayerPrefs.SetInt("musicStatus", 1);
                    MUSICVOL = 1f;
                }
                else
                {
                    PlayerPrefs.SetInt("musicStatus", 0);
                    MUSICVOL = 0;
                }
            }
        }

        #endregion
    }
}