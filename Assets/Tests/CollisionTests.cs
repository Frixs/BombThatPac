using System.Collections;
using Characters;
using Items;
using Managers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

namespace Tests
{
    public class CollisionTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SceneManager.LoadScene(1);
        }

        [SetUp]
        public void SetUp()
        {
            // TODO: it would be better to have method for this.
            GameManager.Instance.DespawnAllUnits();
            Object.DestroyImmediate(GameObject.FindWithTag("Map"));
            Object.Instantiate(Resources.Load("GridTest"));
            MapManager.Instance.TilemapGameplay = GameObject.Find("Tilemap_Gameplay").GetComponent<Tilemap>();
            MapManager.Instance.Setup();
            GameManager.Instance.SpawnAllUnits();

            //_playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerBomberman.prefab");
        }
        
        [TearDown]
        public void TearDown()
        {
            SceneManager.LoadScene(1);
            //UnityEngine.Object.DestroyImmediate(_playerPrefab, true);
        }
        
        [UnityTest]
        public IEnumerator TestCollidePlayerWithPlayer()
        {
            Player p1 = GameManager.Instance.Players[0].PlayerComponent;
            p1.DisableActions();
            Player p2 = GameManager.Instance.Players[1].PlayerComponent;
            p2.DisableActions();
            
            Vector3 targetOriginalPos = new Vector3(p2.transform.position.x, p2.transform.position.y, p2.transform.position.z);

            TransformManager.Instance.AddTransfMoveTowards(p1.gameObject, 0, targetOriginalPos, 3f);
            
            yield return new WaitForSeconds(3f);
            
            if (targetOriginalPos != p2.transform.position)
                Assert.True(true);
            else
                Assert.Fail();
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestCollidePlayerWithGhost()
        {
            Player p1 = GameManager.Instance.Players[0].PlayerComponent;
            p1.DisableActions();
            Vector3 targetOriginalPos = new Vector3(p1.transform.position.x - 2, p1.transform.position.y, p1.transform.position.z);
            Player p2 = GameManager.Instance.Players[1].PlayerComponent;
            p2.gameObject.SetActive(false);
            
            Ghost ghost = GameManager.Instance.Ghosts[0];
            ghost.DisableActions();
            ghost.transform.position = targetOriginalPos;
            
            TransformManager.Instance.AddTransfMoveTowards(p1.gameObject, 0, targetOriginalPos, 3f);
            
            yield return new WaitForSeconds(3f);
            
            if (p1.IsDeath && !p1.gameObject.activeInHierarchy)
                Assert.True(true);
            else
                Assert.Fail();
            
            yield return new WaitForSeconds(3.5f);
            
            Assert.IsTrue(!p1.IsDeath, "Player is not respawned!");

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestCollidePlayerWithFragment()
        {
            Player p1 = GameManager.Instance.Players[0].PlayerComponent;
            p1.DisableActions();
            Vector3 targetOriginalPos = new Vector3(p1.transform.position.x, p1.transform.position.y - 4, p1.transform.position.z);
            Player p2 = GameManager.Instance.Players[1].PlayerComponent;
            p2.gameObject.SetActive(false);
            
            int totalCount = GameObject.Find("Fragments").GetComponentsInChildren<ItemFragment>().Length;
            
            TransformManager.Instance.AddTransfMoveTowards(p1.gameObject, 0, targetOriginalPos, 3f);
            
            yield return new WaitForSeconds(3f);
            
            Assert.IsTrue(GameObject.Find("Fragments").GetComponentsInChildren<ItemFragment>().Length == 0, "No collision. No destroy.");
            Assert.IsTrue(p1.FragmentCounter == 3, "Wrong fragment counter.");
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestCollidePlayerWithWall()
        {
            Player p1 = GameManager.Instance.Players[0].PlayerComponent;
            p1.DisableActions();
            Vector3 targetOriginalPos = new Vector3(p1.transform.position.x, p1.transform.position.y + 3, p1.transform.position.z);
            Player p2 = GameManager.Instance.Players[1].PlayerComponent;
            p2.gameObject.SetActive(false);
            
            TransformManager.Instance.AddTransfMoveTowards(p1.gameObject, 3f, targetOriginalPos, 3f);
            
            yield return new WaitForSeconds(3f);
            
            Assert.IsTrue(p1.transform.position != targetOriginalPos, "No collision.");
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestCollidePlayerWithObstacle()
        {
            Player p1 = GameManager.Instance.Players[0].PlayerComponent;
            p1.gameObject.SetActive(false);
            Player p2 = GameManager.Instance.Players[1].PlayerComponent;
            p2.DisableActions();
            Vector3 targetOriginalPos = new Vector3(p2.transform.position.x, p2.transform.position.y + 2, p2.transform.position.z);
            
            TransformManager.Instance.AddTransfMoveTowards(p2.gameObject, 3f, targetOriginalPos, 3f);
            
            yield return new WaitForSeconds(3f);
            
            Assert.IsTrue(p2.transform.position != targetOriginalPos, "No collision.");
            
            yield return null;
        }
    }
}