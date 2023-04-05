using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] Powerup currentPowerup;
    [SerializeField] float activeDuration=20.0f;

    /*Upon Triggering the Ghost Powerup,
    the Frog's Collider should be
    switched off, and the opacity
    of the sprite should be adjusted
    to half of it's original value.*/

    private void OnTriggerEnter2D(Collider2D col)
    {
        FrogBehavior frog = col.gameObject.GetComponent<FrogBehavior>();

        if (frog != null)
        {
            frog.ResetPowerups();
            switch (currentPowerup)
            {
                case Powerup.Ghost:
                    frog.ToggleGhost();
                    break;
                case Powerup.Floatie:
                    frog.ToggleFloatie();
                    break;
                case Powerup.Missile:
                    frog.AddMissile();
                    break;
                case Powerup.Wall:
                    //frog.PlaceWall();
                    break;
                case Powerup.Health:
                    frog.AddHealth();
                    break;
                case Powerup.Shield:
                    frog.ToggleShield();
                    break;
            }
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        StartCoroutine("ExpireTimer");
    }

    IEnumerator ExpireTimer()
    {
        yield return new WaitForSeconds(activeDuration);
        Destroy(gameObject);
    }
}
