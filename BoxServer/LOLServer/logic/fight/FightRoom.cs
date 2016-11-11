using AceNetFrameWork.ace;
using LOLServer.tool;
using OneByOne;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Constans;

namespace LOLServer.logic.fight
{
    public class FightRoom : AbsMulitHandler, HanderInterface
    {
        public int rowNum = 5;
        public int colNum = 8;

        //队伍玩家映射表
        //int => userId
        public Dictionary<int, FightPlayerModel> teamOne = new Dictionary<int, FightPlayerModel>();
        public Dictionary<int, FightPlayerModel> teamTwo = new Dictionary<int, FightPlayerModel>();

        private List<FightPlayerModel> allPlayer = new List<FightPlayerModel>();

        //队伍建筑映射表
        private Dictionary<int, FightBuildModel> teamOneBuildMap = new Dictionary<int, FightBuildModel>();
        private Dictionary<int, FightBuildModel> teamTwoBuildMap = new Dictionary<int, FightBuildModel>();

        private MapModel map = new MapModel();

        AtomicInt enterCount = new AtomicInt();

        AtomicInt atoid = new AtomicInt();

        private List<int> offline = new List<int>();
        //玩家当前坐标映射
        private ConcurrentDictionary<int, MoveDTO> playerPosition = new ConcurrentDictionary<int, MoveDTO>();

        public void init(SelectModel[] one, SelectModel[] two)
        {
            enterCount.reset();
            offline.Clear();
            teamOne.Clear();
            teamTwo.Clear();
            teamOneBuildMap.Clear();
            teamTwoBuildMap.Clear();
            allPlayer.Clear();
            List<int> wus = new List<int>();
            FightPlayerModel fpm;
            foreach (SelectModel item in one)
            {
                fpm = create(item);
                teamOne.Add(item.userId, fpm);
                allPlayer.Add(fpm);
                wus.Add(item.userId);
            }
            foreach (SelectModel item in two)
            {
                fpm = create(item);
                teamTwo.Add(item.userId, fpm);
                allPlayer.Add(fpm);
                wus.Add(item.userId);
            }
            Console.WriteLine("users" + allPlayer.Count);
            initBuild();
            writeToUsers(wus.ToArray(), Protocol.TYPE_SELECT, 0, SelectProtocol.START_FIGHT_BRO, null);
        }

        void initBuild()
        {
            //初始化建筑 注意 两边箭塔固定为10个  所以这里 做个简单的约束 -1-  -10为 队伍1的箭塔 -11- -20为队伍2的箭塔
            // 各位1 为主基地 为2-4 高级 5-7中级 8-10初级 客户端约束好坐标依次生成即可
            //这里的自减ID看似没有规则 其实初始化为0 ，每次调用-1后返回 而这里是此ID的首次调用 因此会是-1到-20
            //那么开始队伍一的初始化吧
            int id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 1));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 2));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 2));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 2));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 3));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 3));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 3));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 4));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 4));
            id = atoid.getAndReduce();
            teamOneBuildMap.Add(id, createFBM(id, 4));
            //那么开始队伍二的初始化吧
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 1));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 2));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 2));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 2));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 3));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 3));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 3));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 4));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 4));
            id = atoid.getAndReduce();
            teamTwoBuildMap.Add(id, createFBM(id, 4));
        }

        private FightBuildModel createFBM(int id, int code)
        {
            BuildDataModel data = BuildData.buildMap[code];
            FightBuildModel model = new FightBuildModel(id, code, data.hp, data.hp, data.atk, data.def, data.reborn, data.rebornTime, data.initiative, data.infrared, data.name);
            return model;
        }

        FightPlayerModel create(SelectModel model)
        {
            FightPlayerModel player = new FightPlayerModel();
            player.id = model.userId;
            player.heroId = model.heroId;
            player.name = getUser(model.userId).name;
            player.exp = 0;
            player.level = 1;
            player.free = 1;
            player.money = 0;

            player.equs = new int[6];
            //TODO 剩余数据 需要脚本载入
            HeroDataModel data = HeroData.heroMap[model.heroId];
            player.hp = data.hpBase;
            player.maxHp = data.hpBase;
            player.atk = data.atkBase;
            player.def = data.defBase;
            player.aspeed = data.aspeed;
            player.speed = data.speed;
            player.range = data.range;
            player.skills = initSkill(data.skills);
            return player;
        }

        private FightSkill[] initSkill(int[] skills)
        {
            FightSkill[] skill = new FightSkill[skills.Length];

            for (int i = 0; i < skills.Length; i++)
            {
                int skillCode = skills[i];
                SkillDataModel data = SkillData.skillMap[skillCode];
                SkillLevelData lData = data.levels[0];
                FightSkill temp = new FightSkill(skillCode, 0, lData.level, lData.time, lData.mp, lData.range, data.name, data.info, data.target, data.type);
                skill[i] = temp;

            }
            return skill;
        }

        public void MessageRecevie(AceNetFrameWork.ace.UserToken token, AceNetFrameWork.ace.auto.SocketModel message)
        {
            switch (message.command)
            {
                case FightProtocol.ENTER_CREQ:
                    enter(token);
                    break;
                case FightProtocol.MOVE_CREQ:
                    move(token, message.getMessage<MoveDTO>());
                    break;
                case FightProtocol.SKILL_UP_CREQ:
                    skillUp(token, message.getMessage<int>());
                    break;
                case FightProtocol.ATTACK_CREQ:
                    attack(token, message.getMessage<int[]>());
                    break;
                case FightProtocol.DAMAGE_CREQ:
                    damage(token, message.getMessage<DamageDTO>());
                    break;
            }
        }

        private void damage(UserToken token, DamageDTO value)
        {
            AbsFightModel atkPlayer = null;
            int userId = getUserId(token);
            int skillLevel = 0;
            int atkTeam = 0;
            FightModelType atkType;
            if (value.id > 0)
            {
                if (value.id != userId) return;
                atkPlayer = getPlayer(userId);
                if (value.skill > 0)
                {
                    skillLevel = (atkPlayer as FightPlayerModel).skillLevel(value.skill);
                    if (skillLevel == -1)
                    {
                        return;
                    }
                }
                atkTeam = getPlayerTeam(userId);
                atkType = FightModelType.HUMAN;
            }
            else
            {
                //TODO 如果ID小于0的话 则为炮塔或者小兵 需要判断目标是否为用户
                if (value.id >= -20)
                {
                    atkType = FightModelType.BUILD;
                }
                else
                {
                    atkType = FightModelType.HUMAN;
                }
            }

            if (!SkillProcessMap.has(value.skill)) return;



            ISkill skill = SkillProcessMap.get(value.skill);
            List<int[]> damages = new List<int[]>();
            foreach (int[] item in value.targetDamage)
            {
                AbsFightModel beatk = null;
                int beTeam = 0;
                FightModelType beatkType;
                if (item[0] > 0)
                {
                    beatk = getPlayer(item[0]);
                    beTeam = getPlayerTeam(item[0]);
                    beatkType = FightModelType.HUMAN;
                }
                else
                {
                    if (item[0] >= -20)
                    {
                        if (item[0] >= -10)
                        {
                            beatk = teamOneBuildMap[item[0]];
                            beTeam = 1;
                        }
                        else
                        {
                            beatk = teamTwoBuildMap[item[0]];
                            beTeam = 2;
                        }
                        beatkType = FightModelType.BUILD;
                    }
                    else
                    {
                        //TODO这里是小兵了

                        beatkType = FightModelType.HUMAN;
                    }
                }
                skill.damage(skillLevel, ref atkPlayer, ref beatk, atkType, beatkType, atkTeam == beTeam, ref damages);
                if (beatk.hp == 0)
                {
                    if (beatk.id > 0)
                    {
                        //说明是英雄 给予击杀者奖金
                    }
                    else if (beatk.id < -20)
                    {
                        //说明是小兵 给予周围辅助 辅助金
                    }
                    else
                    {
                        //说明是炮台 判断是否拥有复活能力 给予全体奖金
                    }
                }
            }
            DamageDTO dto = new DamageDTO();
            dto.id = value.id;
            dto.skill = value.skill;
            dto.targetDamage = damages.ToArray();
            brocast(FightProtocol.DAMAGE_BRO, dto);
        }

        int getPlayerTeam(int userId)
        {
            if (teamOne.ContainsKey(userId))
            {
                return 1;
            }
            return 2;
        }

        FightPlayerModel getPlayer(int userId)
        {
            if (teamOne.ContainsKey(userId))
            {
                return teamOne[userId];
            }
            return teamTwo[userId];
        }

        GridModel getPlayerGrid(int userId)
        {
            return map.grids.Find(gm => gm.player.Contains(userId));
        }

        private void attack(UserToken token, int[] target)
        {
            AttackDTO atk = new AttackDTO();
            atk.id = getUserId(token);
            atk.target = target;
            brocast(FightProtocol.ATTACK_BRO, atk);
        }

        private void skillUp(UserToken token, int skill)
        {
            int userId = getUserId(token);
            FightPlayerModel player;
            if (teamOne.ContainsKey(userId))
            {
                player = teamOne[userId];
            }
            else
            {
                player = teamTwo[userId];
            }
            if (player.free > 0)
            {
                foreach (FightSkill item in player.skills)
                {
                    if (item.id == skill)
                    {
                        if (item.nextLevel != -1 && item.nextLevel <= player.level)
                        {
                            player.free -= 1;
                            int level = item.level + 1;
                            SkillLevelData data = SkillData.skillMap[skill].levels[level];
                            item.mp = data.mp;
                            item.nextLevel = data.level;
                            item.range = data.range;
                            item.time = data.time;
                            item.level = level;
                            write(token, FightProtocol.SKILL_UP_SRES, item);
                        }
                        return;
                    }
                }
            }
        }

        private void move(UserToken token, MoveDTO dto)
        {
            int userId = getUserId(token);
            move(userId, dto);
        }

        private void move(int userId, MoveDTO dto)
        {
            var gmList = map.grids;
            var TargetGm = gmList.Find(g => g.col == dto.x && g.row == dto.z);
            if (TargetGm == null)
                return;

            if (TargetGm.state == GridState.EMPTY)
            {
                dto.userId = userId;
                dto.y = (int)PlayerState.MOVING;
                playerPosition.AddOrUpdate(userId, dto, (key, oldValue) =>
                {
                    var playerGm = getPlayerGrid(userId);
                    if (playerGm != null && playerGm.player.Count == 1)
                    {
                        playerGm.state = GridState.EMPTY;
                        playerGm.player.Remove(userId);
                    }
                    return oldValue = dto;
                });
                TargetGm.player.Add(userId);
                TargetGm.state = GridState.STAND;
                brocast(FightProtocol.MOVE_BRO, dto);
            }
            else if (TargetGm.state == GridState.STAND)
            {
                setPlayerState(userId, PlayerState.IDEL);
            }
        }

        private void setPlayerState(int userId, PlayerState state)
        {
            var dto = playerPosition[userId];
            dto.userId = userId;
            dto.y = (int)state;
            brocast(FightProtocol.MOVE_BRO, dto);
        }

        private void setMultyPlayerState(PlayerState state, bool aliveOnly)
        {
            Console.WriteLine("set all player to " + state.ToString());

            for (int i = 0; i < allPlayer.Count; i++)
            {
                var p = allPlayer[i];
                if (aliveOnly && p.hp > 0)
                {
                    setPlayerState(p.id, state);
                    continue;
                }

                setPlayerState(p.id, state);
            }
        }

        private new void enter(UserToken token)
        {
            if (entered(token)) return;
            base.enter(token);
            enterCount.getAndAdd();
            Console.WriteLine("entered:" + enterCount.get());
            if (enterCount.get() == teamOne.Count + teamTwo.Count)
            {
                initGame();
                startGame();
            }
        }

        private void initGame()
        {
            FightRoomModel frm = new FightRoomModel();
            frm.teamOne = teamOne.Values.ToArray();
            frm.teamTwo = teamTwo.Values.ToArray();
            frm.teamOneBuildMap = teamOneBuildMap.Values.ToArray();
            frm.teamTwoBuildMap = teamTwoBuildMap.Values.ToArray();
            map = createMap();
            frm.map = map;
            brocast(FightProtocol.FIGHT_BRO, frm);
        }

        private MapModel createMap()
        {
            MapModel mm = new MapModel();
            mm.rolNum = rowNum;
            mm.colNum = colNum;
            mm.grids = new List<GridModel>();
            int obstacleCount = 0;
            int obstacleMax = rowNum;
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    GridModel gm;
                    if (obstacleCount < obstacleMax)
                        gm = (createGrid(i, j, null));
                    else
                        gm = (createGrid(i, j, GridState.EMPTY));
                    if (gm.state == GridState.OBSTACLE)
                        obstacleCount++;
                    mm.grids.Add(gm);
                }
            }

            return mm;
        }

        private GridModel createGrid(int i, int j, GridState? gs = null)
        {
            GridModel gm = new GridModel();
            gm.row = i;
            gm.col = j;
            gm.player = new List<int>();
            if (gs != null)
            {
                gm.state = (GridState)gs;
            }
            //没有指定则随机生成
            else
            {
                var ran = new Random(GetRandomSeed());
                var r = ran.Next(0, colNum);
                gm.state = r == 0 ? GridState.OBSTACLE : GridState.EMPTY;
            }
            return gm;
        }

        #region Game Controller

        System.Timers.Timer logicTimer = new System.Timers.Timer();

        private void startGame()
        {
            //开局12S选定位置
            StartTimer(12, setRandomPosForUnselectPosPlayer_Elapsed);
        }

        private void setRandomPosForUnselectPosPlayer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetTimer(setRandomPosForUnselectPosPlayer_Elapsed);

            for (int i = 0; i < allPlayer.Count; i++)
            {
                var p = allPlayer[i];
                if (!playerPosition.ContainsKey(p.id))
                {
                    var m = GetAValidRandomPos();
                    m.userId = allPlayer[i].id;
                    move(p.id, m);
                }
            }

            StartNextRound();
        }

        private void StartNextRound()
        {
            //todo spawn and brodcast trapssss

            StartTimer(10, setPlayerCanMoveInNewRound_Elapsed);
        }

        private void setPlayerCanMoveInNewRound_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetTimer(setPlayerCanMoveInNewRound_Elapsed);

            setMultyPlayerState(PlayerState.IDEL, true);

            StartTimer(1, doDamageInTrap_Elapsed);
        }

        private void doDamageInTrap_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetTimer(doDamageInTrap_Elapsed);

            setMultyPlayerState(PlayerState.MOVING, true);

            //todo brodcast damage function
            //todo check game over

            StartNextRound();
        }

        private void StartTimer(int seconds, System.Timers.ElapsedEventHandler action)
        {
            logicTimer.Interval = seconds * 1000;
            logicTimer.Elapsed += action;
            logicTimer.Enabled = true;
        }

        private void ResetTimer(System.Timers.ElapsedEventHandler action)
        {
            logicTimer.Elapsed -= action;
            logicTimer.Enabled = false;
        }

        private MoveDTO GetAValidRandomPos()
        {
            var validPos = map.grids.FindAll(g => g.state == GridState.EMPTY);
            if (validPos == null || validPos.Count <= 0)
                return null;
            var ran = new System.Random(GetRandomSeed());
            var r = ran.Next(0, validPos.Count);
            MoveDTO m = new MoveDTO();
            m.x = validPos[r].col;
            m.z = validPos[r].row;
            return m;
        }

        #endregion






        public void ClientClose(AceNetFrameWork.ace.UserToken token)
        {
            base.leave(token);
            int id = getUserId(token);
            if (teamOne.ContainsKey(id) || teamTwo.ContainsKey(id))
            {
                if (!offline.Contains
                    (id))
                {
                    offline.Add(id);
                }
            }
            if (offline.Count == teamOne.Count + teamTwo.Count)
            { //移除战场

                EventUtil.Instance.fightDestory(getArea());
            }
        }

        public override int getType()
        {
            return Protocol.TYPE_FIGHT;
        }

        private int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
