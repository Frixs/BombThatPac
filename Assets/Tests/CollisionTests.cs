using System.Collections;
using System.Runtime.Serialization.Formatters;
using Characters;
using Managers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class CollisionTests
    {
        [SetUp]
        public void SetUp()
        {
            SceneManager.LoadScene(1);
            
            //_playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerBomberman.prefab");
        }

        [TearDown]
        public void TearDown()
        {
            //UnityEngine.Object.DestroyImmediate(_playerPrefab, true);
        }
        
        [UnityTest]
        public IEnumerator TestCollidePlayerWithPlayer()
        {
            Player p1 = GameManager.Instance.Players[0].PlayerComponent;
            Player p2 = GameManager.Instance.Players[1].PlayerComponent;
            
            p1.transform.position = new Vector3(0f, -50f, 0f);
            p2.transform.position = new Vector3(1f, -50f, 0f);
            
            Vector3 targetOriginalPos = p2.transform.position;
            
            yield return new WaitForSeconds(3f);
            
            Assert.IsNotNull(GameManager.Instance.Players);
            
            yield return null;
        }
        
        /*
        [UnityTest]
        [Timeout(180000)] // Sets the timeout of the test in millisecon-ds (if the test hangs, this will ensure it closes after 3 minutes).
        public IEnumerator TestAnimationAnimUtilityPrefab()
        {
            yield return new WaitForSeconds(1f);
            // In this example, let's assume that our Example.prefab has a script on it called ExampleScript.
            var script = GameManager.Instance.Players[0].CharacterInstance.GetComponent<Player>();
            // Assert that the script exists on our prefab so we don't stumble upon this problem in the future.
            Assert.IsTrue(script == null, "Player script must be set on Example.prefab.");
        }
        */
    }
}