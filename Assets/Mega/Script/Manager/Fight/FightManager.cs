using EGamePlay;
using EGamePlay.Combat;
using ET;

namespace Mega
{
    public class FightManager : GameComponent
    {
        private bool isFighting = false;

        /// <summary>
        /// 初始化战斗管理器
        /// </summary>
        public void Init()
        {
            MasterEntity.Create();
            // Entity.Create<TimerComponent>();
            Entity.Create<CombatContext>();
            isFighting = true;
        }

        /// <summary>
        /// 战斗结束回收资源
        /// </summary>
        public void End()
        {
            isFighting = false;
            Entity.Destroy(MasterEntity.Instance);
            MasterEntity.Destroy();
        }

        private void Update()
        {
            if (!isFighting)
            {
                return;
            }

            MasterEntity.Instance.Update();
            // TimerComponent.Instance.Update();
        }
    }
}