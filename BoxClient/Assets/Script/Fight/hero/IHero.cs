﻿using OneByOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


    public interface IHero
    {
        void attack(GameObject[] targets);
        void attacked();

        void skill(int code,GameObject[] targets);

        void skill(int code, Transform target);

        void skilled();
        void setHp(GameObject hp, Transform par);

        void StartMove(MoveDTO dto);
        void setTag(string tag);

        void damage(int value);

        FightPlayerModel getData();

        void setData(FightPlayerModel model);

        GameObject getHpObj();

        Constans.PlayerState getState();
    }

