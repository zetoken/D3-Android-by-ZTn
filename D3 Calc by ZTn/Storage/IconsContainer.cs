using ZTn.BNet.D3;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Medias;
using ZTn.BNet.D3.Skills;

namespace ZTnDroid.D3Calculator.Storage
{
    public class IconsContainer
    {
        public D3Picture ActiveSkill1;
        public D3Picture ActiveSkill2;
        public D3Picture ActiveSkill3;
        public D3Picture ActiveSkill4;
        public D3Picture ActiveSkill5;
        public D3Picture ActiveSkill6;

        public D3Picture PassiveSkill1;
        public D3Picture PassiveSkill2;
        public D3Picture PassiveSkill3;
        public D3Picture PassiveSkill4;

        public D3Picture Head;
        public D3Picture Torso;
        public D3Picture Feet;
        public D3Picture Hands;
        public D3Picture Shoulders;
        public D3Picture Legs;
        public D3Picture Bracers;
        public D3Picture MainHand;
        public D3Picture OffHand;
        public D3Picture Waist;
        public D3Picture RightFinger;
        public D3Picture LeftFinger;
        public D3Picture Neck;

        /// <summary>
        /// Fetches the icon of a given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static D3Picture FetchIconOf(ItemSummary item)
        {
            if (item == null || item.Icon == null)
            {
                return null;
            }

            return D3Api.GetItemIcon(item.Icon, "large");
        }

        /// <summary>
        /// Fetches the icon of a given active skill.
        /// </summary>
        /// <param name="activeSkill"></param>
        /// <returns></returns>
        private static D3Picture FetchIconOf(ActiveSkill activeSkill)
        {
            if (activeSkill == null || activeSkill.skill == null || activeSkill.skill.icon == null)
            {
                return null;
            }

            return D3Api.GetSkillIcon(activeSkill.skill.icon, "64");
        }

        /// <summary>
        /// Fetches the icon of a given passive skill.
        /// </summary>
        /// <param name="passiveSkill"></param>
        /// <returns></returns>
        private static D3Picture FetchIconOf(PassiveSkill passiveSkill)
        {
            if (passiveSkill == null || passiveSkill.skill == null || passiveSkill.skill.icon == null)
            {
                return null;
            }

            return D3Api.GetSkillIcon(passiveSkill.skill.icon, "64");
        }

        /// <summary>
        /// Fetches the icons of all hero active skills.
        /// </summary>
        /// <param name="skills"></param>
        public void FetchActiveSkillIcons(ActiveSkill[] skills)
        {
            if (skills == null)
            {
                return;
            }

            ActiveSkill1 = FetchIconOf(skills[0]);
            ActiveSkill2 = FetchIconOf(skills[1]);
            ActiveSkill3 = FetchIconOf(skills[2]);
            ActiveSkill4 = FetchIconOf(skills[3]);
            ActiveSkill5 = FetchIconOf(skills[4]);
            ActiveSkill6 = FetchIconOf(skills[5]);
        }

        /// <summary>
        /// Fetches the icons of all hero items.
        /// </summary>
        /// <param name="items"></param>
        public void FetchItemIcons(HeroItems items)
        {
            if (items == null)
            {
                return;
            }

            Head = FetchIconOf(items.head);
            Torso = FetchIconOf(items.torso);
            Feet = FetchIconOf(items.feet);
            Hands = FetchIconOf(items.hands);
            Shoulders = FetchIconOf(items.shoulders);
            Legs = FetchIconOf(items.legs);
            Bracers = FetchIconOf(items.bracers);
            MainHand = FetchIconOf(items.mainHand);
            OffHand = FetchIconOf(items.offHand);
            Waist = FetchIconOf(items.waist);
            RightFinger = FetchIconOf(items.rightFinger);
            LeftFinger = FetchIconOf(items.leftFinger);
            Neck = FetchIconOf(items.neck);
        }

        /// <summary>
        /// Fetches the icons of all hero passive skills.
        /// </summary>
        /// <param name="skills"></param>
        public void FetchPassiveSkillIcons(PassiveSkill[] skills)
        {
            if (skills == null)
            {
                return;
            }

            var passiveCount = skills.Length;
            if (passiveCount >= 1)
            {
                PassiveSkill1 = FetchIconOf(skills[0]);
            }
            if (passiveCount >= 2)
            {
                PassiveSkill2 = FetchIconOf(skills[1]);
            }
            if (passiveCount >= 3)
            {
                PassiveSkill3 = FetchIconOf(skills[2]);
            }
            if (passiveCount >= 4)
            {
                PassiveSkill4 = FetchIconOf(skills[3]);
            }
        }
    }
}