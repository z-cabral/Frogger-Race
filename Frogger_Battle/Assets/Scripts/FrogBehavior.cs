using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehavior : MonoBehaviour
{
    [SerializeField] KeyCode
        keyUp=KeyCode.W,
        keyDown=KeyCode.S,
        keyLeft=KeyCode.A,
        keyRight=KeyCode.D;
    [SerializeField] bool
        floatie=false,
        pitfallable=true,
        ghost=false,
        shield=false,
        invincible=false;
    [SerializeField] int
        missile,
        powerupDur=5,
        invincibilityDur=5,
        xBoundOffset=2,
        yBoundOffset=-3;
    [SerializeField] Transform
        _cameraTransform,
        _startTransform,
        _otherFrogPos;
    [SerializeField] float
        _camPosX,
        _camPosY;
    [SerializeField] Material
        MatStandard,
        MatShield,
        MatGhost,
        MatFloatie,
        MatInvincibility;
    [SerializeField] AudioClip
        jumpClip,
        deathClip,
        powerupClip,
        noMorePowerClip;

    Renderer _renderer;

    int frog;

    private void Awake()
    {
        if (this.gameObject.name == "FrogL")
        {
            frog = 1;
        }
        else if (this.gameObject.name == "FrogR")
        {
            frog = 2;
        }
        //caching start pos
        _startTransform = this.gameObject.transform;
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        _camPosX = _cameraTransform.position.x;
        _camPosY = _cameraTransform.position.y;

        if (GameBehavior.Instance.CurrentState == State.Play)
        {
            FrogMovement();
        }
    }

    private void LateUpdate()
    {
        if(GameBehavior.Instance.CurrentState == State.Title)
        {
            this.gameObject.transform.position = _startTransform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!invincible || !shield)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle") && ghost != true)
            {
                LoseLife(true);
            }
            else if (col.gameObject.layer == LayerMask.NameToLayer("Pitfall") && floatie != true && transform.parent == null)
            {
                LoseLife(true);
            }
        }
    }

    private void FrogMovement()
    {
        //if lower than the camera and move, lose a life
        if (this.transform.position.y <= GameBehavior.Instance.yBoundDown + _camPosY + yBoundOffset)
        {
            LoseLife(true);
        }
        //When Key is down, subtract 1 to y value,
        //rotate transform to face direction moved
        //If the frog's y position is less than the ydown bound,
        //Don't let the frog continue
        if (Input.GetKeyDown(keyDown))
        {
            if(this.transform.position.y >= GameBehavior.Instance.yBoundDown + _camPosY + yBoundOffset)
            {
                Move(Vector3.down, 180f);
            }
        }
        //When Key is down, add 1 to y value,
        //rotate transform to face direction moved
        else if (Input.GetKeyDown(keyUp))
        {
            Move(Vector3.up, 0f);
        }
        //When Key is down, subtract 1 to x value,
        //rotate transform to face direction moved.
        //additionally, if the frog's x value is less
        //than (or equal to) the game window's boundaries:
        //teleport the frog to the other side of the boundaries.
        else if (Input.GetKeyDown(keyLeft))
        {
            Move(Vector3.left, 90f);

            if (this.transform.position.x < -GameBehavior.Instance.xBound+xBoundOffset)
            {

                this.transform.position = new Vector3(
                    GameBehavior.Instance.xBound - xBoundOffset,
                    this.transform.position.y,
                    this.transform.position.z);
            }
        }
        //When Key is down, add 1 to x value,
        //rotate transform to face direction moved.
        //additionally, if the frog's x value is greater
        //than (or equal to) the game window's boundaries:
        //teleport the frog to the other side of the boundaries.
        else if (Input.GetKeyDown(keyRight))
        {
            Move(Vector3.right, -90f);
            if (this.transform.position.x > GameBehavior.Instance.xBound-xBoundOffset)
            {

                this.transform.position = new Vector3(
                    -GameBehavior.Instance.xBound + xBoundOffset,
                    this.transform.position.y,
                    this.transform.position.z);
            }
        }

    }

    private void Move(Vector3 direction, float rotation)
    {
        AudioBehavior.Instance.RandomSFX(jumpClip);

        Vector3 destination = transform.position + direction;

        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));
        Collider2D pitfall = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Pitfall"));

        if (platform != null)
        {
            transform.SetParent(platform.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        if (obstacle != null && ghost != true && (invincible != true || shield != true))
        {
            LoseLife(true);
        }
        else if (pitfall != null && platform == null && (invincible != true || shield != true))
        {
            LoseLife(true);
        }
        else
        {
            StartCoroutine(Leap(destination));
        }

        this.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
     
    IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0.0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
    }
    //Toggle powerups
    public void ResetPowerups()
    {
        if (floatie==true)
            floatie=false;
        if (pitfallable==false)
            pitfallable=true;
        if (ghost==true)
            ghost=false;
        if (shield==true)
            shield=false;
        if (invincible==true)
            invincible=false;
    }

    public void ToggleShield()
    {
        shield = !shield;

        if (shield == true)
        {
            StopAllCoroutines();
            AudioBehavior.Instance.RandomSFX(powerupClip);
            StartCoroutine(ShieldTimer());
        }
        else if(shield == false)
        {
            AudioBehavior.Instance.RandomSFX(noMorePowerClip);
            StopCoroutine(ShieldTimer());
        }
    }

    public void ToggleGhost()
    {
        ghost = !ghost;

        if (ghost == true)
        {
            StopAllCoroutines();
            AudioBehavior.Instance.RandomSFX(powerupClip);
            StartCoroutine(GhostTimer());
        }
        else if (ghost == false)
        {
            AudioBehavior.Instance.RandomSFX(noMorePowerClip);
            StopCoroutine(GhostTimer());
        }
    }

    public void ToggleFloatie()
    {
        floatie = !floatie;

        if (floatie == true)
        {
            StopAllCoroutines();
            AudioBehavior.Instance.RandomSFX(powerupClip);
            StartCoroutine(FloatieTimer());
        }
        else if (floatie == false)
        {
            AudioBehavior.Instance.RandomSFX(noMorePowerClip);
            StopCoroutine(FloatieTimer());
        }
    }

    public void AddHealth()
    {
        if(GameBehavior.Instance.Frogs[frog - 1].Lives != 3)
        {
            AudioBehavior.Instance.RandomSFX(powerupClip);
            GameBehavior.Instance.Frogs[frog - 1].Lives++;
        }
    }

    public void Toggleinvincibility()
    {
        invincible = !invincible;

        if (invincible == true)
        {
            StopAllCoroutines();
            StartCoroutine(InvincibilityTimer());
        }
        else if (invincible == false)
        {
            AudioBehavior.Instance.RandomSFX(noMorePowerClip);
            StopCoroutine(InvincibilityTimer());
        }
    }

    IEnumerator ShieldTimer()
    {
        _renderer.material = MatShield;

        yield return new WaitForSeconds(powerupDur);

        int flicker = 0;
        while (flicker < 4)
        {
            _renderer.material = flicker % 2 == 0 ? MatStandard : MatShield;

            flicker++;

            yield return new WaitForSeconds(powerupDur * 0.5f / 4);
        }

        flicker = 0;

        while (flicker < 4)
        {
            _renderer.material = flicker % 2 == 0 ? MatStandard : MatShield;

            flicker++;

            yield return new WaitForSeconds(powerupDur * 0.5f / 8);
        }

        _renderer.material = MatStandard;

        ToggleShield();
    }

    IEnumerator GhostTimer()
    {
        _renderer.material = MatGhost;

        yield return new WaitForSeconds(powerupDur);

        int flicker = 0;
        while (flicker < 4)
        {
            _renderer.material = flicker % 2 == 0 ? MatStandard : MatGhost;

            flicker++;

            yield return new WaitForSeconds(powerupDur * 0.5f / 4);
        }

        flicker = 0;

        while (flicker < 4)
        {
            _renderer.material = flicker % 2 == 0 ? MatStandard : MatGhost;

            flicker++;

            yield return new WaitForSeconds(powerupDur * 0.5f / 8);
        }

        _renderer.material = MatStandard;
        ToggleGhost();
    }

    IEnumerator FloatieTimer()
    {
        _renderer.material = MatFloatie;

        yield return new WaitForSeconds(powerupDur);

        int flicker = 0;
        while (flicker < 4)
        {
            _renderer.material = flicker % 2 == 0 ? MatStandard : MatFloatie;

            flicker++;

            yield return new WaitForSeconds(powerupDur * 0.5f / 4);
        }

        flicker = 0;

        while (flicker < 4)
        {
            _renderer.material = flicker % 2 == 0 ? MatStandard : MatFloatie;

            flicker++;

            yield return new WaitForSeconds(powerupDur * 0.5f / 8);
        }

        _renderer.material = MatStandard;
        ToggleFloatie();
    }

    IEnumerator InvincibilityTimer()
    {
        _renderer.material = MatInvincibility;

        yield return new WaitForSeconds(invincibilityDur);

        int flicker = 0;
        while (flicker < 4)
        {
            _renderer.material = flicker % 2 == 0 ? MatStandard : MatInvincibility;

            flicker++;

            yield return new WaitForSeconds(invincibilityDur * 0.5f / 4);
        }

        flicker = 0;

        while (flicker < 4)
        {
            _renderer.material = flicker % 2 == 0 ? MatStandard : MatInvincibility;

            flicker++;

            yield return new WaitForSeconds(invincibilityDur * 0.5f / 8);
        }

        _renderer.material = MatStandard;
        Toggleinvincibility();
    }

    public void AddMissile()
    {
        if(missile <= 2)
        {
            missile++;
        }
    }

    //upon Losing a life, subtract a life,
    //then 
    private void LoseLife(bool Respawns)
    {
        GameBehavior.Instance.Frogs[frog - 1].Lives--;
        //respawn
        GameBehavior.Instance.UpdateLives();

        AudioBehavior.Instance.RandomSFX(deathClip);

        if(Respawns)
            Respawn();
    }

    private void Respawn()
    {
        Toggleinvincibility();
        this.transform.position = _otherFrogPos.position;
    }
}
