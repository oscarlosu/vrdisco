using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RecordingSystem
{
    public class RecordingRepoHandler : MonoBehaviour
    {
        public static RecordingRepoHandler Instance { get; private set; }

        [SerializeField]
        private string _repoFileName = "recordings";

        private RecordingRepo _repo;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Load in the repo from file or create new repo.
            if (!LoadRepo(_repoFileName))
            {
                _repo = new RecordingRepo();
            }
        }

        public void AddRecording(Recording recording)
        {
            _repo.Recordings.Add(recording);
        }

        public bool LoadRepo(string fileName)
        {
            string filePath = Application.dataPath + "/" + fileName + ".json";

            if (!File.Exists(filePath))
            {
                // Return false, if the file does not exist.
                return false;
            }

            _repo = JsonUtility.FromJson<RecordingRepo>(File.ReadAllText(filePath));
            return true;
        }

        public void SaveRepo(string fileName)
        {
            string filePath = Application.dataPath + "/" + fileName + ".json";

            File.WriteAllText(filePath, JsonUtility.ToJson(_repo, false));

            Debug.Log("Saved repo to: " + filePath);
        }

        public List<Recording> GetMusicPieceRecordings(int musicPieceId)
        {
            return _repo.Recordings.Where(r => r.MusicPieceId == musicPieceId).ToList();
        }

        [ContextMenu("Load repo from default file")]
        public void LoadDefaultRepo()
        {
            LoadRepo(_repoFileName);
        }

        [ContextMenu("Save repo to default file")]
        public void SaveDefaultRepo()
        {
            SaveRepo(_repoFileName);
        }

        [ContextMenu("Print all recordings")]
        private void PrintAllRecordings()
        {
            Debug.Log(string.Join("\n", _repo.Recordings.Select(r => r.ToString()).ToArray()));
        }
    }
}