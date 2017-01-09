using UnityEngine;
using System.Collections;
using System;

public enum CreatureState
{
    Idle,
    Wander,
    LookAround,
    Captured,
    Unactive
}

public class Creature : EventDispatcherBase
{

    public string type = "Basic";
    public float baseSpeed = 0.1f;
    public float modSpeed = 0.1f;
    public float baseAgility = 2f;
    public float modAgility = 2f;
    public float mobility = 20f;
    public float SpecialOdds = 0.8f; //to not come
    public bool HasSpecial = false;
    public Transform MyEffect;
    public CreatureState State;
    protected float SpecialLifeTime = 10f;
    protected Transform Effect;
    protected Rigidbody2D RB2D;
    protected Renderer render;
    protected bool isWrappingX = false;
    protected bool isWrappingY = false;
    protected Camera cam;
    protected GameObject beetle;
    protected Animator animator;
    protected float Angle;
    public int creatureID;
    public int UID;
    public float scale = 1.0f;
    // Use this for initialization
    void Start () {
        
        render = gameObject.AddComponent<SpriteRenderer>();
        cam = Camera.main;      
        RB2D = gameObject.AddComponent<Rigidbody2D>();
        RB2D.gravityScale = 0f;
        type = gameObject.name;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        switch (State)
        {
            case CreatureState.Idle:
                Idle();
                break;
            case CreatureState.Wander:
                Wander();
                break;
            case CreatureState.LookAround:
                LookAround();
                break;
            case CreatureState.Captured:
                Captured();
                break;
            default:
                break;
        }
    }

    // OnSpawned called by pool manager 
    public void OnSpawned()
    {
        if(animator == null)
        {
            beetle = transform.Find("beetle").gameObject;
            animator = beetle.GetComponent<Animator>();
        }

        animator.Play("Birth");
        State = CreatureState.Idle;
        Angle = UnityEngine.Random.Range(0f, 360f);
    }

    // OnDespawned called by pool manager 
    public void OnDespawned()
    {
        //Debug.Log("OnDespawned");
        FreezeMovement();
        animator.Stop();
        State = CreatureState.Unactive;
    }

    public void Die()
    {
        animator.SetBool("isDead", true);
        Invoke("DeSpawn", 1f);
        RemoveEffect();
    }

    public void Special()
    {
        HasSpecial = true;
        Effect = F3DPool.instance.Spawn(MyEffect, transform.position, Quaternion.identity, transform);
        Invoke("RemoveEffect", SpecialLifeTime);
    }

    protected void RemoveEffect()
    {
        if (HasSpecial)
        {
            HasSpecial = false;
            Effect.gameObject.GetComponent<ParticleSystem>().Stop();
            F3DPool.instance.Despawn(Effect);
            //Effect = null;
            eventDispatcher.dispatchEvent( new CreatureEvent(CreatureEvent.ON_SPECIAL_COMPLETE) );
            if (IsInvoking("RemoveEffect"))
                CancelInvoke("RemoveEffect");
        }
    }

    protected void DeSpawn()
    {
        F3DPool.instance.Despawn(transform);
    }

    private void Captured()
    {
        throw new NotImplementedException();
    }

    private void LookAround()
    {
        //GetAngle
        FreezeMovement();

        
        float newAngle = UnityEngine.Random.Range(Angle - mobility, Angle + mobility);
        Vector2 DirVector = Vector2FromAngle(newAngle);

        GetDirection(newAngle);
        RB2D.AddForce(DirVector.normalized * (baseSpeed + UnityEngine.Random.Range(0, modSpeed)));
        Angle = newAngle;
        Invoke("LookAround", UnityEngine.Random.Range(baseAgility, modAgility));
        State = CreatureState.Wander;
    }

    protected void FreezeMovement()
    {
        RB2D.isKinematic = true;
        RB2D.velocity = Vector2.zero;
        RB2D.angularVelocity = 0f;
        RB2D.isKinematic = false;
    }

    protected Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    protected void GetDirection(float angle)
    {
        angle = angle % 360;
        if (angle < 0)
        {
            angle += 360;
        }

        if(angle > 0f && angle < 90f|| angle > 270f)
        {
            transform.localScale = new Vector2(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y);
            //Debug.Log("angle:" + angle + " " + gameObject.name);
        }
        else
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    protected float AngleFromVector2(Vector2 v)
    {
        float a = Mathf.Tan(v.x / v.y);
        a *= Mathf.Rad2Deg;
        return a;
    }

    private void Wander()
    {
        ScreenWrap();
    }

    protected void ScreenWrap()
    {
        if (!gameObject.activeSelf)
            return;

        if (render.isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;

            return;
        }

        if(isWrappingX && isWrappingY)
        {
            return;
        }      

        Vector3 newPosition = transform.position;
        Vector2 viewportPosition = cam.WorldToViewportPoint(newPosition);


        if (!isWrappingX)
        {
            if(viewportPosition.x > 1)
            {
                newPosition.x = cam.ViewportToWorldPoint(Vector2.zero).x;
                isWrappingX = true;
            }
            else if(viewportPosition.x < 0)
            {
                newPosition.x = cam.ViewportToWorldPoint(Vector2.one).x;
                isWrappingX = true;
            }
        }

        if (!isWrappingY)
        {
            if (viewportPosition.y > 1)
            {
                newPosition.y = cam.ViewportToWorldPoint(Vector2.zero).y;
                isWrappingY = true;
            }
            else if (viewportPosition.y < 0)
            {
                newPosition.y = cam.ViewportToWorldPoint(Vector2.one).y;
                isWrappingY = true;
            }
        }

        transform.position = newPosition;
    }

    private void HitWall()
    {

    }

    private void Idle()
    {
        //Animate idle
        State = CreatureState.LookAround;
    }
}
