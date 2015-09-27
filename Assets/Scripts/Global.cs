using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.OurUtils;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.SavedGame;

public class Global : MonoBehaviour
{
    public static float AdsTime = Time.time;
    public static int KillCount = 0, BlockCount = 0;

    public static bool LoggedIn = false;
    public const bool PlayGamesDebugLogsEnabled = false;
    private static bool mAuthenticating = false;

    public static bool Authenticated
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }

    public static void SignOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public static void ShowLeaderboardUI()
    {
        if (Authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }

    public static void ShowAchievementsUI()
    {
        if (Authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    public static void SendAchievement(long blocks)
    {
        if (Authenticated)
        {
            if (blocks >= 1000)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQDg", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
            if (blocks >= 500)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQDQ", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
            if (blocks >= 300)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQCQ", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
            if (blocks >= 200)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQCA", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
            if (blocks >= 100)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQBw", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
            if (blocks >= 80)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQBg", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
            if (blocks >= 50)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQBQ", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
            if (blocks >= 30)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQAw", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
            if (blocks >= 10)
            {
                Social.ReportProgress("CgkI7cCOyswOEAIQAQ", 100, (bool success) =>
                {
                    if (success)
                    {
                        // TODO
                    }
                });
            }
        }
    }

    public static void SaveScore(long score, long blocks, long time)
    {
        if (Authenticated)
        {
            Social.ReportScore(score, "CgkI7cCOyswOEAIQCw", (bool success) =>
            {
                // handle success or failure
            });

            Social.ReportScore(blocks, "CgkI7cCOyswOEAIQCg", (bool success) =>
            {
                // handle success or failure
            });

            Social.ReportScore(time, "CgkI7cCOyswOEAIQDA", (bool success) =>
            {
                // handle success or failure
            });
        }
    }
    public static void Authenticate()
    {
        if (Authenticated || mAuthenticating)
        {
            Debug.LogWarning("Ignoring repeated call to Authenticate().");
            return;
        }
        PlayGamesPlatform.DebugLogEnabled = PlayGamesDebugLogsEnabled;

        PlayGamesPlatform.Activate();

        ((PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI("CgkI7cCOyswOEAIQCw");

        mAuthenticating = true;

        Social.localUser.Authenticate((bool success) =>
        {
            mAuthenticating = false;

            if (success)
            {
                Debug.Log("Login successful!");
            }
            else
            {
                Debug.LogWarning("Failed to sign in with Google Play Games.");
            }
        });
    }
}
