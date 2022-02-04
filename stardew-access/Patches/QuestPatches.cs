﻿using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Quests;

namespace stardew_access.Patches
{
    internal class QuestPatches
    {
        private static string currentDailyQuestText = " ";

        #region For Special Orders Board
        internal static void SpecialOrdersBoardPatch(SpecialOrdersBoard __instance)
        {
            try
            {
                int x = Game1.getMouseX(), y = Game1.getMouseY(); // Mouse x and y position

                if (__instance.acceptLeftQuestButton.visible && __instance.acceptLeftQuestButton.containsPoint(x, y))
                {
                    string toSpeak = getSpecialOrderDetails(__instance.leftOrder);

                    toSpeak = $"Left Quest:\n\t{toSpeak}\n\tPress left click to accept this quest.";

                    MainClass.ScreenReader.SayWithMenuChecker(toSpeak, true);
                    return;
                }

                if (__instance.acceptRightQuestButton.visible && __instance.acceptRightQuestButton.containsPoint(x, y))
                {
                    string toSpeak = getSpecialOrderDetails(__instance.rightOrder);

                    toSpeak = $"Right Quest:\n\t{toSpeak}\n\tPress left click to accept this quest.";

                    MainClass.ScreenReader.SayWithMenuChecker(toSpeak, true);
                    return;
                }
            }
            catch (Exception e)
            {
                MainClass.Monitor.Log($"Unable to narrate Text:\n{e.Message}\n{e.StackTrace}", LogLevel.Error);
            }
        }

        private static string getSpecialOrderDetails(SpecialOrder order)
        {
            int daysLeft = order.GetDaysLeft();
            string description = order.GetDescription();
            string objectiveDescription = "";
            string name = order.GetName();
            int moneyReward = order.GetMoneyReward();

            // Get each objectives
            for (int i = 0; i < order.GetObjectiveDescriptions().Count; i++)
            {
                objectiveDescription += order.GetObjectiveDescriptions()[i] + ", \n";
            }

            string toReturn = $"{name}\n\tDescription:{description}\n\tObjectives: {objectiveDescription}";

            if (order.IsTimedQuest())
            {
                toReturn = $"{toReturn}\n\tTime: {daysLeft} days";
            }

            if (order.HasMoneyReward())
            {
                toReturn = $"{toReturn}\n\tReward: {moneyReward}g";
            }

            return toReturn;
        }
        #endregion

        #region For Normal Billboard in the town
        internal static void BillboardPatch(Billboard __instance, bool ___dailyQuestBoard)
        {
            try
            {
                if (!___dailyQuestBoard)
                {
                    #region Callender
                    for (int i = 0; i < __instance.calendarDays.Count; i++)
                    {
                        if (__instance.calendarDays[i].containsPoint(Game1.getMouseX(), Game1.getMouseY()))
                        {
                            string toSpeak = $"Day {i + 1}";

                            if (__instance.calendarDays[i].name.Length > 0)
                            {
                                toSpeak += $", {__instance.calendarDays[i].name}";
                            }
                            if (__instance.calendarDays[i].hoverText.Length > 0)
                            {
                                toSpeak += $", {__instance.calendarDays[i].hoverText}";
                            }

                            if (Game1.dayOfMonth == i + 1)
                                toSpeak += $", Current";

                            MainClass.ScreenReader.SayWithChecker(toSpeak, true);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Daily Quest Board
                    if (Game1.questOfTheDay == null || Game1.questOfTheDay.currentObjective == null || Game1.questOfTheDay.currentObjective.Length == 0)
                    {
                        // No quests
                        string toSpeak = "No quests for today!";
                        if (currentDailyQuestText != toSpeak)
                        {
                            currentDailyQuestText = toSpeak;
                            MainClass.ScreenReader.Say(toSpeak, true);
                        }
                    }
                    else
                    {
                        SpriteFont font = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ko) ? Game1.smallFont : Game1.dialogueFont);
                        string description = Game1.parseText(Game1.questOfTheDay.questDescription, font, 640);
                        string toSpeak = description;

                        if (currentDailyQuestText != toSpeak)
                        {
                            currentDailyQuestText = toSpeak;

                            // Snap to accept quest button
                            if (__instance.acceptQuestButton.visible)
                            {
                                toSpeak += "\t\n Left click to accept quest.";
                                __instance.acceptQuestButton.snapMouseCursorToCenter();
                            }

                            MainClass.ScreenReader.Say(toSpeak, true);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                MainClass.Monitor.Log($"Unable to narrate Text:\n{e.Message}\n{e.StackTrace}", LogLevel.Error);
            }
        }
        #endregion

        #region Journal Menu
        internal static void QuestLogPatch(QuestLog __instance, int ___questPage, List<List<IQuest>> ___pages, int ___currentPage, IQuest ____shownQuest, List<string> ____objectiveText)
        {
            try
            {
                bool snapMouseToRewardBox = false;

                if (___questPage == -1)
                {
                    #region Quest Lists
                    for (int i = 0; i < __instance.questLogButtons.Count; i++)
                    {
                        if (___pages.Count() > 0 && ___pages[___currentPage].Count() > i)
                        {
                            string name = ___pages[___currentPage][i].GetName();
                            int daysLeft = ___pages[___currentPage][i].GetDaysLeft();
                            string toSpeak = $"{name} quest";

                            if (daysLeft > 0 && ___pages[___currentPage][i].ShouldDisplayAsComplete())
                                toSpeak += $"\t\n {daysLeft} days left";

                            toSpeak += ___pages[___currentPage][i].ShouldDisplayAsComplete() ? " completed!" : "";
                            if (__instance.questLogButtons[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
                            {
                                MainClass.ScreenReader.SayWithChecker(toSpeak, true);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Individual quest
                    string description = Game1.parseText(____shownQuest.GetDescription(), Game1.dialogueFont, __instance.width - 128);
                    string title = ____shownQuest.GetName();
                    string toSpeak = " ";
                    if (____shownQuest.ShouldDisplayAsComplete())
                    {
                        #region Quest completed menu

                        toSpeak = $"Quest: {title} Completed!";

                        if (__instance.HasReward())
                        {
                            snapMouseToRewardBox = true;
                            if (__instance.HasMoneyReward())
                            {
                                toSpeak += $"you recieved {____shownQuest.GetMoneyReward()}g";
                            }

                            toSpeak += "... left click to collect reward";
                        }

                        #endregion
                    }
                    else
                    {
                        #region Quest in-complete menu
                        toSpeak = $"Title: {title}. \t\n Description: {description}";

                        for (int j = 0; j < ____objectiveText.Count; j++)
                        {
                            if (____shownQuest != null)
                            {
                                _ = ____shownQuest is SpecialOrder;
                            }
                            string parsed_text = Game1.parseText(____objectiveText[j], width: __instance.width - 192, whichFont: Game1.dialogueFont);

                            toSpeak += $"\t\nOrder {j + 1}: {parsed_text} \t\n";
                        }

                        if (____shownQuest != null)
                        {
                            int daysLeft = ____shownQuest.GetDaysLeft();

                            if (daysLeft > 0)
                                toSpeak += $"\t\n{daysLeft} days left.";
                        }
                        #endregion
                    }

                    // Move mouse to reward button
                    if (snapMouseToRewardBox)
                        __instance.rewardBox.snapMouseCursorToCenter();

                    MainClass.ScreenReader.SayWithChecker(toSpeak, true);
                    #endregion
                }
            }
            catch (Exception e)
            {
                MainClass.Monitor.Log($"Unable to narrate Text:\n{e.Message}\n{e.StackTrace}", LogLevel.Error);
            }
        }
        #endregion


        internal static void resetGlobalVars()
        {
            currentDailyQuestText = " ";
        }
    }
}
