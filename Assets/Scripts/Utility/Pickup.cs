using Managers;
using ScriptableObjects;
using System.Collections;
using UnityEngine;
using Utility;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private DialogueScriptableObject _dialogue;
    [SerializeField]
    private PickupScriptableObject _pickup;
    [SerializeField]
    private GameObject _popup;

    private void Awake()
    {
    }

    private void Start()
    {
    }

    public void ShowPopUp()
    {
        _popup.SetActive(true);
    }

    public void Interact()
    {
        HidePopUp();
        IEnumerator InteractCoroutine()
        {
            GameObject player = GameObject.FindWithTag(Tags.PLAYER_TAG);
            PlayerInventory _inventory = player.GetComponent<PlayerInventory>();
            PlayerController _player_controller = player.GetComponent<PlayerController>();
            CountersManager _counters_manager = GameObject.FindWithTag(Tags.LOGIC_TAG).GetComponent<CountersManager>();
            DialogueManager _dialogue_manager = GameObject.FindWithTag(Tags.DIALOGUE_MANAGER_TAG).GetComponent<DialogueManager>();
            GameUIManager _game_ui_manager = GameObject.FindWithTag(Tags.LOGIC_TAG).GetComponent<GameUIManager>();
            DexManagerUi _dex_ui = GameObject.FindWithTag(Tags.DEX_TAG).GetComponent<DexManagerUi>();

            if (_inventory.CanAdd())
            {
                yield return StartCoroutine(_dialogue_manager.ReadDialogue(_dialogue));
                _player_controller.DisableMovement();
                int inventory_index = _inventory.AddPickup(_pickup);
                _dex_ui.AddSeed(_pickup);
                _game_ui_manager.SetInventorySpriteAtIndex(inventory_index, _pickup._sprite);
                _counters_manager.IncreaseOxygenDecrementStep(_pickup._weight);
                _player_controller.PlayInteractAnimation();
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return StartCoroutine(_dialogue_manager.ReadDialogue(_inventory._no_space_dialogue));
            }
            _player_controller.EnableMovement();
            ShowPopUp();
        }
        StartCoroutine(InteractCoroutine());
    }

    public void HidePopUp()
    {
        _popup.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            ShowPopUp();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            HidePopUp();
    }
}
