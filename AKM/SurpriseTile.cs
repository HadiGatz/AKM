using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    class SurpriseTile : Tile
    {
        string[] SurpriseTexts;
        Random rd = new Random();

        public SurpriseTile(string name) : base(name)
        {
            this.name = name;
            SurpriseTexts = new string[] { "You go to the local school’s car wash fundraiser – but you beat your kids to death!",
                "You help your neighbor bring in her groceries. She makes you lunch to say thanks!",
                "You set aside time every week to hang out with your elderly neighbor – you’ve heard some amazing stories!",
                "You organize a bake sale for your local school. ",
                "You volunteer your art skills and paint a mural at the local school!",
                "Your fuzzy friends at the animal shelter will be happy for your donation.",
                "You didn’t shop local!",
                "You volunteer at your local literacy center and learn some fun phrases in a new language! ",
                "You organize a family reunion!",
                "You volunteer as a homework helper, and you learn some stuff, too!",
                "You volunteered at a blood drive. There were free cookies!"};
        }

        public int GenerateSurpriseMoney(Player player)
        {
            int randomNum = rd.Next(1, 200);
            int minusOrPlus = rd.Next(1, 3);
            if (minusOrPlus == 1)
                player.SetMoney(player.GetMoney() + randomNum);
                
            else
                player.SetMoney(player.GetMoney() - randomNum);
            return randomNum;
        }
        public string RandomSurpriseString()
        {
            int randomIndex = rd.Next(0, this.SurpriseTexts.Length);
            return this.SurpriseTexts[randomIndex];
        }
    }
}
