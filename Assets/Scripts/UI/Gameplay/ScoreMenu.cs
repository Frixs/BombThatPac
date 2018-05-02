using Managers;
using TMPro;
using UnityEngine;

namespace UI.Gameplay
{
    public class ScoreMenu : MonoBehaviour
    {
        /// <summary>
        /// Player name placeholder text reference.
        /// </summary>
        [SerializeField] public TMP_Text PlacerNamePlaceholderText;

        /// <summary>
        /// Formatted paragraph with stat labels. 
        /// </summary>
        [SerializeField] private TMP_Text _statsLabelParagraph;

        /// <summary>
        /// Formatted paragraph with stat values.
        /// </summary>
        [SerializeField] private TMP_Text _statsValueParagraph;

        public void MainMenuButtonEvent()
        {
            gameObject.SetActive(false);
            SceneLoadingManager.Instance.LoadScene(0);
        }

        public void QuitButtonEvent()
        {
            Application.Quit();
        }

        private void OnEnable()
        {
            CountStatistics();
        }

        /// <summary>
        /// Count statistics and print them.
        /// </summary>
        public void CountStatistics()
        {
            _statsLabelParagraph.text = "Player kills:\n" +
                                        "Ghost kills:\n" +
                                        "Deaths:\n" +
                                        "Collected items:";

            int[] playerKillCount = new int[2];
            int[] ghostKillCount = new int[2];
            int[] deathCount = new int[2];
            int[] itemCollectCount = new int[2];

            for (int i = 0; i < GameManager.Instance.Players.Length; i++)
            {
                playerKillCount[i] = GameManager.Instance.Players[i].PlayerComponent.PlayerKillCount;
                ghostKillCount[i] = GameManager.Instance.Players[i].PlayerComponent.GhostKillCount;
                deathCount[i] = GameManager.Instance.Players[i].PlayerComponent.DeathCount;
                itemCollectCount[i] = GameManager.Instance.Players[i].PlayerComponent.ItemCollectCount;
            }

            int numberLength = 3;

            _statsValueParagraph.text = playerKillCount[0].ToString().PadRight(numberLength - playerKillCount[0].ToString().Length > 0 ? numberLength - playerKillCount[0].ToString().Length : 0, ' ') +
                                        " : " +
                                        playerKillCount[1].ToString().PadLeft(numberLength - playerKillCount[1].ToString().Length > 0 ? numberLength - playerKillCount[1].ToString().Length : 0, ' ') + "\n" +
                                        ghostKillCount[0].ToString().PadRight(numberLength - ghostKillCount[0].ToString().Length > 0 ? numberLength - ghostKillCount[0].ToString().Length : 0, ' ') +
                                        " : " +
                                        ghostKillCount[1].ToString().PadLeft(numberLength - ghostKillCount[1].ToString().Length > 0 ? numberLength - ghostKillCount[1].ToString().Length : 0, ' ') + "\n" +
                                        deathCount[0].ToString().PadRight(numberLength - deathCount[0].ToString().Length > 0 ? numberLength - deathCount[0].ToString().Length : 0, ' ') +
                                        " : " +
                                        deathCount[1].ToString().PadLeft(numberLength - deathCount[1].ToString().Length > 0 ? numberLength - deathCount[1].ToString().Length : 0, ' ') + "\n" +
                                        itemCollectCount[0].ToString().PadRight(numberLength - itemCollectCount[0].ToString().Length > 0 ? numberLength - itemCollectCount[0].ToString().Length : 0, ' ') +
                                        " : " +
                                        itemCollectCount[1].ToString().PadLeft(numberLength - itemCollectCount[1].ToString().Length > 0 ? numberLength - itemCollectCount[1].ToString().Length : 0, ' ');
        }
    }
}