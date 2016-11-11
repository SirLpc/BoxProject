using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
   public class SkillProcessMap
    {
       static Dictionary<int, ISkill> skills = new Dictionary<int, ISkill>();

       static SkillProcessMap() {
           push(-1, new SkillNomal());
       }

       static void push(int code, ISkill skill)
       {
           skills.Add(code, skill);
       }

     public static bool has(int code) {
           return skills.ContainsKey(code);
       }
     public static ISkill get(int code) {
         return skills[code];
     }
    }
}
