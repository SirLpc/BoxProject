﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
    public class HeroData
    {
       public  static readonly Dictionary<int, HeroDataModel> heroMap = new Dictionary<int, HeroDataModel>();

        static HeroData()
        {
            create(1, "阿狸", 2, 1, 5000, 300, 5, 2, 30, 10, 1, 0.5f,6, 1, 2, 3, 4);

            create(2, "阿木木", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f,3, 1, 2, 3, 4);
            create(3, "埃希", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 1,6, 2, 3, 4);
            create(4, "盲僧", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 1,3, 2, 3, 4);
        }

        static void create(int id,
        string name,
        int atkBase,
        int defBase,
        int hpBase,
        int mpBase,
        int atkUp,
        int defUp,
        int hpUp,
        int mpUp,
            float speed,
            float aspeed,
            float range,
        params int[] skills)
        {
            HeroDataModel model = new HeroDataModel();
            model.id = id;
            model.name = name;
            model.atkBase = atkBase;
            model.defBase = defBase;
            model.hpBase = hpBase;
            model.mpBase = mpBase;
            model.atkUp = atkUp;
            model.defUp = defUp;
            model.hpUp = hpUp;
            model.mpUp = mpUp;
            model.speed = speed;
            model.aspeed = aspeed;
            model.range = range;
            model.skills = skills;
            heroMap.Add(id, model);
        }
    }

   public partial class HeroDataModel
    {
       public int id;
       public string name;
       public int atkBase;
       public int defBase;
       public int hpBase;
       public int mpBase;
       public int atkUp;
       public int defUp;
       public int hpUp;
       public int mpUp;
       public float speed;
       public float aspeed;
       public float range;
       public int[] skills;
    }
}
