namespace Mega
{
    public enum LoadEvent
    {
        SUCCESS,
        ERROR
    }

    /// <summary>
    /// 这里的ViewID要和UI预制体的名字保持一致
    /// </summary>
    public enum ViewID : int
    {
        UIMenu,

        UITestDebuger,
        UITestSound,
        UITestSpine,
        UITestFight,
        UIFight,
        UITestTextEffect,
    }

    public enum ViewType
    {
        Normal = 0,
        Dialog = 100,
        Hint   = 200,
        Top    = 300,
        Effect = 400
    }

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

    public enum SceneType
    {
        Start,
        Login,
        Hall,
        Game,

        Sound,
    }

    public enum EventId
    {
        TEST_EventNormal,
        TEST_EventParamInt,
        TEST_EventParamObject,
    }

    public enum GamePlayType
    {
        None,
        LootGrid,
    }
}