using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constans
{
    public  enum SkillTarget
    {
        SELF,//自身
        F_H,//友方英雄
        F_N_N,//友方非建筑
        F_ALL,//友方全体
        E_H,//敌方英雄
        E_N_N,//敌方非建筑
        E_S_N//敌方小兵或中立单位

    }
    /// <summary>
    /// 技能释放类型
    /// </summary>
    public enum SkillType
    {
        SELF,//自身
        TARGET,//目标模式
        POSITION//指定点(方向)释放  定点和方向 操作模式上一致 不同的是 定点就是在指定点实例化或者向指定点移动到点销毁，方向就是朝指定点的方向移动 自身位移距离
    }
    /// <summary>
    /// 战斗角色种类，暂定为建筑和生物两类
    /// </summary>
    public enum FightModelType { 
        BUILD,//建筑
        HUMAN//生物
    }
    /// <summary>
    /// 位置的状态
    /// </summary>
    public enum GridState
    {
        /// <summary>
        /// 空地
        /// </summary>
        EMPTY,
        /// <summary>
        /// 有人占用
        /// </summary>
        STAND,
        /// <summary>
        /// 障碍
        /// </summary>
        OBSTACLE,
        /// <summary>
        /// 陷阱
        /// </summary>
        TRAP,
    }
    public enum PlayerState
    {
        /// <summary>
        /// 游戏开始的初始化，还未在地图上
        /// </summary>
        INIT,
        /// <summary>
        /// 空闲
        /// </summary>
        IDEL,
        /// <summary>
        /// 移动中
        /// </summary>
        MOVING,
        ATK,DAMAGE,
        /// <summary>
        /// 死亡
        /// </summary>
        DEAD,
    }
}
