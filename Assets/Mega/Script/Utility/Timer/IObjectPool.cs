using System;

public delegate void CallBackObjPool();

public interface IObjectPool
{
    event CallBackObjPool PoolShrinked;
    event CallBackObjPool MemoryUseOut;

    int MinObjCount { get; }

    int MaxObjCount { get; }

    int CurObjCount { get; }

    int IdleObjCount { get; }

    bool Initialize(Type objType, object[] cArgs, int minNum, int maxNum);

    object RentObject();

    void GiveBackObject(int objHashCode);

    void Dispose();
}