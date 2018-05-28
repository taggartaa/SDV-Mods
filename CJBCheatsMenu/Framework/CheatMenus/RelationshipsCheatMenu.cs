using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    internal class RelationshipsCheatMenu : CheatMenu
    {
        public override string Id => "CBJCheatsMenu_RelationshipsCheatMenu";

        public override string Title => this.I18n.Get("tabs.relationships");

        public RelationshipsCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
            : base(config, cheats, i18n)
        {

        }

        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();

                Menu.OptionGroup relationshipsOptionGroup = new Menu.OptionGroup($"{this.I18n.Get("relationships.title")}:");
                relationshipsOptionGroup.Options.Add(new Menu.OptionCheckbox(this.I18n.Get("relationships.give-gifts-anytime"), this.Config.AlwaysGiveGift, value => this.Config.AlwaysGiveGift = value));
                relationshipsOptionGroup.Options.Add(new Menu.OptionCheckbox(this.I18n.Get("relationships.no-decay"), this.Config.NoFriendshipDecay, value => this.Config.NoFriendshipDecay = value));
                relationshipsOptionGroup.Options.Add(new Menu.Option($"{this.I18n.Get("relationships.friends")}:"));


                List<string> nonRelationshipCharacters = new List<string>
                    { "???", "Bouncer", "Marlon", "Gil", "Gunther" };
                foreach (StardewValley.NPC npc in StardewValley.Utility.getAllCharacters())
                {
                    if (StardewValley.Game1.player.friendshipData.ContainsKey(npc.Name) &&
                        (npc.Name != "Sandy" || StardewValley.Game1.player.mailReceived.Contains("ccVault")) &&
                         !nonRelationshipCharacters.Contains(npc.Name) &&
                         !npc.IsMonster &&
                         !(npc is StardewValley.Characters.Horse) &&
                         !(npc is StardewValley.Characters.Pet))
                    {
  
                        relationshipsOptionGroup.Options.Add(new NPCHeartPicker(npc));
                    }
                   
                }
                optionGroups.Add(relationshipsOptionGroup);

                return optionGroups;
            }
        }

        public class NPCHeartPicker : Menu.IOptionHeartPicker
        {
            public StardewValley.NPC NPC { get; set; }

            public NPCHeartPicker(StardewValley.NPC npc)
            {
                this.NPC = npc;
            }

            public int Value
            {
                get
                {
                    int heartLevel = 0;
                    if (StardewValley.Game1.player.friendshipData.TryGetValue(this.NPC.Name, out StardewValley.Friendship friendship))
                    {
                        heartLevel = Math.Min(10, friendship.Points / StardewValley.NPC.friendshipPointsPerHeartLevel);
                    }

                    return Math.Max(0, heartLevel);
                }
                set
                {
                    if (StardewValley.Game1.player.friendshipData.TryGetValue(this.NPC.Name, out StardewValley.Friendship friendship))
                    {
                        friendship.Points = value * StardewValley.NPC.friendshipPointsPerHeartLevel;
                    }
                }
            }

            public string Label => NPC.displayName;

            public bool Disabled => false;
        }
    }
}
