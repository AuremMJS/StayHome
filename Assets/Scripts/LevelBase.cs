using ColorSnipersU.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace ColorSnipersU.MonoBehaviors.MainGameScene
{

    /// <summary>
    /// Scriptable Object to represent a Level
    /// </summary>
    [CreateAssetMenu(fileName = "Level", menuName = "Level/Base", order = 1)]
    public class LevelBase : ScriptableObject
    {
        // Name of the Level
        public string LevelName;

        // Available Wall Types in this level
        public GameObject[] Walls;

        // Represents how this level is shown in level selection scene
        public GameObject LevelWall;

        // Speed of the level
        public float Speed;

        // Is tutorial available in this level
        public bool isTrainerAllowed;

        // The number of Stars to unlock this level
        public int StarsToUnlock;

        // Is this Level Locked
        public bool Locked;
        public AchievementSO Achievement;
        public Sprite AdventureTitle;

        public int NoOfWallsToTrain;
        public int count;
        public int wallLimit;
        public GameObject WinnerWall;

        public bool isLoadStatic;

        public double WallMovementSpeed;

        private int noOfEmptyWalls;

        public LevelBase(string mLevelName)
        {
            LevelName = mLevelName;
        }
        public void DisplayLevelName()
        {
            Debug.Log(LevelName);
        }


        public GameObject InstantiateWall(Text ScoresMeter, out bool isNoWindowWall)
        {
            int wallNo = 0;
            GameObject wall;
            isNoWindowWall = true;
            if (count < wallLimit)
            {
                if (count < NoOfWallsToTrain && isTrainerAllowed)
                {
                    wallNo = (count) < Walls.Length ? count : Walls.Length - 1;
                }
                else
                    wallNo = UnityEngine.Random.Range(0, Walls.Length);

                //if (!isTrainerAllowed)
                //{
                if (wallNo == 0)
                    noOfEmptyWalls++;
                else
                    noOfEmptyWalls = 0;

                if (noOfEmptyWalls >= 3)
                {
                    wallNo = UnityEngine.Random.Range(1, Walls.Length);
                }
                //}
                wall = Instantiate(Walls[wallNo]);
                wall.GetComponent<WallBase>().ScoresMeter = ScoresMeter;

                //if (tutoCount < NoOfWallsToTrain && count < NoOfWallsToTrain)
                if (count < NoOfWallsToTrain)
                {
                    if (wall.GetComponent<WallBase>().IsTrainerControllable)
                    {
                        wall.GetComponent<WallBase>().shouldPauseForTrainer = isTrainerAllowed;
                        TrainerController.Instance.SwitchTrainerOn(isTrainerAllowed, IncrementCount);
                        wall.GetComponent<WallBase>().SetRotation(count);
                    }
                }
                else
                {
                    TrainerController.Instance.SwitchTrainerOn(false);
                    wall.GetComponent<WallBase>().shouldPauseForTrainer = false;
                    wall.GetComponent<WallBase>().RandomizeWallRotation();
                    wall.GetComponent<WallBase>().RandomizeWindowPosition();
                }
                if (wallNo != 0 || wall.GetComponent<WallBase>().shouldPauseForTrainer)
                {
                    if (wallNo != 0)
                    {
                        isNoWindowWall = false;
                        count++;
                    }
                }
            }
            else
            {
                wall = Instantiate(WinnerWall);
                isNoWindowWall = false;

            }
            return wall;
        }

        private void IncrementCount()
        {
            //tutoCount++;
        }

    }

}
