﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


   public class ResourceLoad
    {
       public static Sprite getHead(string name) {
           return Resources.Load<Sprite>("Texture/GlobalTex/ChampionTex/Icons/"+name);
       }
       public static GameObject getHeroModel(int code)
       {
           return Resources.Load<GameObject>("Hero/" + code);
       }

       public static Sprite getSkillIcon(string name)
       {
           return Resources.Load<Sprite>("Texture/Skillicon/"+name);
       }
       public static GameObject getBar()
       {
           return Resources.Load<GameObject>("HP");
       }
       public static GameObject getLight()
       {
           return Resources.Load<GameObject>("light");
       }
       public static GameObject getHpUp() {
           return Resources.Load<GameObject>("hpUp");
       }
    }

