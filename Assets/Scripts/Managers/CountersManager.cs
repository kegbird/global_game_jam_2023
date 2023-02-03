using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Managers
{
    public class CountersManager : MonoBehaviour
    {
        [SerializeField]
        private float _oxygen_level;
        [SerializeField]
        private float _water_level;
        [SerializeField]
        private float _energy_level;
        [SerializeField]
        private float _oxygen_decrement_step;

        private IEnumerator _oxygen_coroutine;
        private IEnumerator _water_coroutine;
        private IEnumerator _energy_coroutine;

        private void Awake()
        {
        }

        private void Start()
        {
            _oxygen_level = Constants.DEFAULT_OXYGEN_LEVEL;
            _water_coroutine = WaterCounter();
            _energy_coroutine = EnergyCounter();
            StartCoroutine(_water_coroutine);
            StartCoroutine(_energy_coroutine);
        }

        public void StartOxygenCounter()
        {
            _oxygen_coroutine = OxygenCounter();
            StartCoroutine(_oxygen_coroutine);
        }

        public void StopOxygenCounter()
        {
            StopCoroutine(_oxygen_coroutine);
        }

        private IEnumerator OxygenCounter()
        {
            _oxygen_level = Constants.DEFAULT_OXYGEN_LEVEL;
            do
            {
                yield return new WaitForSeconds(Constants.OXYGEN_DECREMENT_DELAY);
                _oxygen_level -= _oxygen_decrement_step;
            }
            while (_oxygen_level > 0);

            yield return StartCoroutine(GameOverAndDie());
        }

        private IEnumerator EnergyCounter()
        {
            _energy_level = Constants.DEFAULT_ENERGY_LEVEL;
            do
            {
                yield return new WaitForSeconds(Constants.ENERGY_DECREMENT_DELAY);
                _energy_level -= Constants.DEFAULT_ENERGY_DECREMENT_STEP;
            }
            while (_energy_level > 0);

            yield return StartCoroutine(GameOver());
        }

        public IEnumerator WaterCounter()
        {
            _water_level = Constants.DEFAULT_WATER_LEVEL;
            do
            {
                yield return new WaitForSeconds(Constants.ENERGY_DECREMENT_DELAY);
                _water_level -= Constants.DEFAULT_WATER_DECREMENT_STEP;
            }
            while (_water_level > 0);

            yield return StartCoroutine(GameOver());
        }
        
        private IEnumerator GameOverAndDie()
        {
            StopAllCoroutines();
            GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG);
            player.GetComponent<PlayerController>().Die();

            GameUIManager game_ui_manager = GameObject.Find(Tags.LOGIC_TAG).GetComponent<GameUIManager>();
            yield return StartCoroutine(game_ui_manager.ShowBlackScreen());
            SceneManager.LoadScene((int)SceneEnum.BAD_ENDING_SCENE);
        }

        private IEnumerator GameOver()
        {
            StopAllCoroutines();
            GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG);
            player.GetComponent<PlayerController>().DisableMovement();

            GameUIManager game_ui_manager = GameObject.Find(Tags.LOGIC_TAG).GetComponent<GameUIManager>();
            yield return StartCoroutine(game_ui_manager.ShowBlackScreen());
            SceneManager.LoadScene((int)SceneEnum.BAD_ENDING_SCENE);
        }

        public void IncreaseOxygenDecrementStep(float increment)
        {
            _oxygen_decrement_step += increment;
        }

        public void IncreaseWaterLevel()
        {
            _water_level += Constants.WATER_PICKUP_INCREMENT;
        }

        public void IncreaseEnergyLevel()
        {
            _energy_level += Constants.ENERGY_PICKUP_INCREMENT;
        }
    }
}