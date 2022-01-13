﻿namespace Mega
{
    #region UI
    
    public enum ViewType
    {
        Normal = 0,
        Dialog = 100,
        Hint   = 200,
        Top    = 300,
        Effect = 400
    }

    #endregion

    #region Scene

    public enum SceneType
    {
        Start,
        Login,
        Hall,
        Game,

        Sound,
    }

    #endregion

    #region Event

    public enum EventId
    {
        TEST_EventNormal,
        TEST_EventParamInt,
        TEST_EventParamObject,
    }

    #endregion

    #region Audio

    public enum SoundType
    {
        Tick,
        Tock,
        Max,
    }

    public enum MusicType
    {
        MondayCigarette,
        Thaw,
        TheLastGoodbye,
        TimeIsRunningOut,
        TunedToLive,
        Max,
    }

    #endregion

    public enum GamePlayType
    {
        None,
        LootGrid,
    }
}