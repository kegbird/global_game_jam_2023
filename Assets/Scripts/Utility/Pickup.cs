using Managers;
using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Accessibility;
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
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowPopUp()
    {
        _animator.SetBool("show", true);
    }

    public void Interact()
    {
        _animator.SetBool("show", false);
        IEnumerator InteractCoroutine()
        {
            CountersManager _counters_manager = GameObject.FindWithTag(Tags.LOGIC_TAG).GetComponent<CountersManager>();
            DialogueManager _dialogue_manager = GameObject.FindWithTag(Tags.DIALOGUE_MANAGER_TAG).GetComponent<DialogueManager>();
            yield return StartCoroutine(_dialogue_manager.ReadDialogue(_dialogue));
            GameObject player = GameObject.FindWithTag(Tags.PLAYER_TAG);
            PlayerInventory _inventory = player.GetComponent<PlayerInventory>();
            PlayerController _player_controller = player.GetComponent<PlayerController>();
            _inventory.AddPickup(_pickup);
            _player_controller.PlayInteractAnimation();
            _counters_manager.IncreaseOxygenDecrementStep(_pickup._weight);
            this.gameObject.SetActive(false);
        }
        StartCoroutine(InteractCoroutine());
    }

    public void HidePopUp()
    {
        _animator.SetBool("show", false);
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
