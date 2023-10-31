using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Collisions : MonoBehaviour
{
    //container for gameObject that keeps track of enemies currently alive
    public GameObject enemyCheck;
    public Text chestCounter;
    public Text lifeCounter;
    public Text energyCounter;
    public Text restartText;
    public AudioSource src;
    public AudioClip sfx1, sfx2, sfx3, sfx4;
    public Transform spawn;

    private CharacterController cc;
    private int chestCount;
    private int lifeCount;
    private int energyCount;
    //states where player can defeat enemies after collecting 3 energy bars or all chests
    private bool energized;
    private bool superEnergized;

    void Start()
    {
        restartText.text = "";

        energized = false;
        energyCount = 0;
        energyCounter.text = "Energy: " +energyCount.ToString()+"/3";

        chestCount = 3;
        chestCounter.text = "Chests Remaining: " + chestCount.ToString();

        lifeCount = 3;
        lifeCounter.text = "lives: " + lifeCount.ToString();

        cc = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        //win condition will check to see if all chests are collected, and enemies are defeated
        if (superEnergized && !enemyCheck.activeSelf)
        {
            gameObject.SetActive(false);
            restartText.text = "You Win!";
            Invoke("Restart", 3);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //trigger event for energy bar
        if (other.CompareTag("Energy"))
        {
            src.clip = sfx1;
            src.Play();
            other.gameObject.SetActive(false);
            energyCount++;
            energyCounter.text = "Energy: " + energyCount.ToString() + "/3";
            //will start a coroutine function that keeps energized state temporary
            if(energyCount == 3)
            {
                //player tag is changed so that enemy ai will flee
                transform.gameObject.tag = "Invulnerable";
                energized = true;
                StartCoroutine(test());
            }
        }

        if (other.CompareTag("Treasure"))
        {
            src.clip = sfx2;
            src.Play();
            other.gameObject.SetActive(false);
            chestCount--;
            chestCounter.text = "Chests Remaining: " + chestCount.ToString();
            if(chestCount == 0)
            {
                //will give player permanent power up to defeat all the gaurds and win the game
                superEnergized = true;
                transform.gameObject.tag = "Invulnerable";
            }
        }

        if (other.CompareTag("Enemy"))
        {
            //checks for powered up state during enemy collision
            //will deafeat the enemy if player is in energized state
            if (energized || superEnergized)
            {
                src.clip = sfx3;
                src.Play();
                other.gameObject.SetActive(false);
            }
            //if player isnt energized, they lose a life and respawn
            else
            {
                lifeCount--;
                src.clip = sfx4;
                src.Play();
                gameObject.SetActive(false);
                transform.position = spawn.position;
                gameObject.SetActive(true);
                lifeCounter.text = "lives: " + lifeCount.ToString();
                //when all lives are lost, game will restart momentarily
                if (lifeCount == 0)
                {
                    gameObject.SetActive(false);
                    restartText.text = "Game Over...";
                    Invoke("Restart", 3);
                }
            }
        }

    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator test()
    {
        //corountine counts downs for the energized state which is also shown in game at energy count
        //once the count goes back down to 0, player is vulnerable again
        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(1);
            energyCount--;
            energyCounter.text = "Energy: " + energyCount.ToString() + "/3";
        }
        energized = false;
        //in case the player becomes superEnergized during the corountine, 'invulnerable' tag is still kept
        //player tag reverts back otherwise
        if (!superEnergized)
        {
            transform.gameObject.tag = "Player";
        }
    }

}
