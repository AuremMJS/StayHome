using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using UnityEngine;

namespace ColorSnipersU.Utilities
{
    public static class SavedGamesHelper
    {
        private static ISavedGameMetadata currentGame = null;
        private static bool isSaving = false;
        private static byte[] readData;
        private static byte[] writeData;

        public static string Read(string filename)
        {
            isSaving = false;
            ReadSavedGame(filename);
            return System.Text.Encoding.UTF8.GetString(readData);
        }

        public static void Write(string filename, string Data)
        {
            isSaving = true;
            writeData = System.Text.Encoding.UTF8.GetBytes(Data);
            ReadSavedGame(filename);
        }
        private static void ReadSavedGame(string filename)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(
                filename,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                OnReadSavedGamesComplete);
        }

        private static void OnReadSavedGamesComplete(SavedGameRequestStatus status, ISavedGameMetadata game)
        {


            if (status == SavedGameRequestStatus.Success)
            {
                // Read the binary game data
                currentGame = game;
                PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game,
                                                    OnReadBinaryData);
            }
        }

        private static void OnReadBinaryData(SavedGameRequestStatus status, byte[] data)
        {
            Debug.Log("(Color Snipers U) Saved Game Binary Read: " + status.ToString());
            if (status == SavedGameRequestStatus.Success)
            {
                try
                {
                    readData = data;
                    if (isSaving)
                        WriteSavedGame(writeData);
                }
                catch (Exception e)
                {
                    Debug.Log("(Color Snipers U) Saved Game Write: convert exception");
                }
            }
        }

        private static void OnWriteSavedGames(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            Debug.Log("(Color Snipers U) Saved Game Write: " + status.ToString());
        }

        private static void WriteSavedGame(byte[] Data)
        {
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder()
                .WithUpdatedPlayedTime(TimeSpan.FromMinutes(currentGame.TotalTimePlayed.Minutes + 1))
                .WithUpdatedDescription("Saved at: " + System.DateTime.Now);

            // You can add an image to saved game data (such as as screenshot)
            // byte[] pngData = <PNG AS BYTES>;
            // builder = builder.WithUpdatedPngCoverImage(pngData);

            SavedGameMetadataUpdate updatedMetadata = builder.Build();

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.CommitUpdate(currentGame, updatedMetadata, Data, OnWriteSavedGames);
        }
    }
}

