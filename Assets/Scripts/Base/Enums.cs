namespace Mega
{
    #region UI

    public enum ViewType
    {
        Normal = 0,   // 一般全屏UI
        Dialog = 100, // 弹窗UI
        Hint   = 200, // Toast 飘字UI
        Top    = 300, // 必须放在最上层的UI
        Effect = 400  // 特效
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
        ChangeLanguage,
        RestLanguageKey,

        TEST_EventNormal,
        TEST_EventParamInt,
        TEST_EventParamObject,

        INVOKE_UI_TOAST_TIPS,
        INVOKE_UI_TOAST_ERROR_TIPS
    }

    #endregion

    #region Audio

    public enum SoundType
    {
        Tick,
        Tock,
        Max
    }

    public enum MusicType
    {
        MondayCigarette,
        Thaw,
        TheLastGoodbye,
        TimeIsRunningOut,
        TunedToLive,
        Max
    }

    #endregion

    public enum GamePlayType
    {
        None,
        LootGrid,
    }
}