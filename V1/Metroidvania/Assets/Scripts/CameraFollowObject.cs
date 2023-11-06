using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollowObject : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;


    private Player player;

    private bool facingRight;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<Player>();

        facingRight = player.facingRight;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
    }





}



