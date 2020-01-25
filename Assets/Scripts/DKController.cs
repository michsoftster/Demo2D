using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DKController : MonoBehaviour
{
  
    public float dkSpeed;
    public float dkJumpForce;
    public int countItem;
    //audio
    public AudioClip[] sfx;
    private AudioSource dkSource;

    Touch touch;
    private SpriteRenderer dkSprite;
    private Rigidbody2D rbDk;
    private Vector2 swipeVector;
    private Vector2 startTouchPosition;
    private bool Ground;
    private int countDamage;
    private Animator dkAnimator;
    // Start is called before the first frame update

    void Awake()
    {
        
    }

    void Start()
    {
        PlayerProfiler.instance.LoadData();
        dkSprite = GetComponent<SpriteRenderer>();
        dkAnimator = GetComponent<Animator>();
        rbDk = GetComponent<Rigidbody2D>();
        dkSource = GetComponent<AudioSource>();
        Ground = false;
        //if (!PlayerPrefs.HasKey("Items"))
        //{
        //    countItem = 0;
        //}
        //else
        //{
        //    //countItem = PlayerPrefs.GetInt("Items");
        //}
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE
        dkAnimator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        rbDk.velocity = new Vector2(dkSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), rbDk.velocity.y);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rbDk.AddForce(Vector2.up * dkJumpForce, ForceMode2D.Impulse);
            dkAnimator.SetTrigger("Jump");
            
        }

#endif

#if UNITY_ANDROID || UNITY_IOS 
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Stationary)
            {
                if (touch.position.x > Screen.width * 0.5f)
                {
                    dkAnimator.SetFloat("Speed", 1);
                    rbDk.velocity = new Vector2(dkSpeed * Time.deltaTime * Vector2.right.x, rbDk.velocity.y);
                    dkSprite.flipX = false;

                }

                if (touch.position.x < Screen.width * 0.5f)
                {
                    dkAnimator.SetFloat("Speed", 1);
                    rbDk.velocity = new Vector2(-dkSpeed * Time.deltaTime * Vector2.right.x, rbDk.velocity.y);
                    dkSprite.flipX = true;

                }

            }

            if (touch.phase == TouchPhase.Ended && Ground == true)
            {
                swipeVector = startTouchPosition - touch.position;
                print("El swipe vale" + swipeVector);
                if (Math.Abs(swipeVector.x) < 1.0f)
                {
                    rbDk.AddForce(Vector2.up * dkJumpForce, ForceMode2D.Impulse);
                    dkAnimator.SetTrigger("Jump");
                    dkSource.PlayOneShot(sfx[0]);
                    Ground = false;
                }
            }



        }

#endif
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.transform.tag == "Ground")
        {
            Ground = true;
            print("Puedo saltar");
        }
        if (col.transform.tag == "Barrel")
        {
            dkAnimator.SetTrigger("Damage");
            DataLoader.instance.currentPlayer.lives--;
            dkSource.PlayOneShot(sfx[2]);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Item")
        {
            countItem=countItem+1;
            DataLoader.instance.currentPlayer.items++;
            dkSource.PlayOneShot(sfx[1]);
            PlayerProfiler.instance.itemsCount = countItem;
            PlayerProfiler.instance.SaveData();
            //PlayerPrefs.SetInt("Items", countItem);
        }

    }
}
