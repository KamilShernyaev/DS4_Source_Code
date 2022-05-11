using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class DatabaseAccess : MonoBehaviour
    {
        public UserData userNameData;
        MongoClient client = new MongoClient("mongodb+srv://Grouli123:Grouli1234@cluster0.rgyjr.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;

        private void Start() 
        {
            database = client.GetDatabase("HighScoreDB");
            collection = database.GetCollection<BsonDocument>("HighScoreCollectionThree");
            SaveScoreToDateBase(userNameData.userName, userNameData.userScore);
        }

        public async void SaveScoreToDateBase(string userName, string score)
        {
            var document = new BsonDocument { { userName, score } };
            await collection.InsertOneAsync(document);
        }

        public async Task<List<HighScore>> GetScoresFromDataBase() 
        {
            var allScoresTask = collection.FindAsync(new BsonDocument());
            var scoresAwaited = await allScoresTask;

            List<HighScore> highscores = new List<HighScore>();
            foreach (var score in scoresAwaited.ToList())
            {
                highscores.Add(Deserialize(score.ToString()));
            }

            return highscores;
        }

        private HighScore Deserialize(string rawJson)
        {
            var highScore = new HighScore();

            var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),") + 4);
            var username = stringWithoutID.Substring(0, stringWithoutID.IndexOf(":") - 2);
            var score = stringWithoutID.Substring(stringWithoutID.IndexOf(":") + 2, stringWithoutID.IndexOf("}")-stringWithoutID.IndexOf(":")-3);
            highScore.UserName = username;
            highScore.Score = score.ToString();
            return highScore;
        }
    }

    public class HighScore
    {
        public string UserName { get; set; }
        public string Score { get; set; }
    }
}