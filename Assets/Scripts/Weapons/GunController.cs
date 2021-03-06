﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool IsShooting
    {
        get
        {
            return isShooting;
        }
    }

    public Sprite CrosshairImage
    {
        get
        {
            return crosshairImage;
        }
    }

    [SerializeField] protected int damage = 10;
    [SerializeField] protected float shotDelay = 1f;
    [SerializeField] protected float ballSpeed = 20f; // TODO maybe give this into the GetBall() function from GameManager
    //[Range(0, 100), SerializeField] protected float critchance = 10f;
    //[SerializeField] protected float recoil = 1f;
    [SerializeField] protected float numberOfShots = 1f;
    [Range(0, 100), SerializeField] protected float accuracy = 5f;
    [SerializeField] Transform mainMuzzle;
    bool isShooting = false;
    BoxCollider coll;
    GameObject owner;
    Camera ownerCam;

    [SerializeField] Sprite crosshairImage;

    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
    }

    public virtual void Equip(GameObject owned)
    {
        owner = owned;
        ownerCam = owner.GetComponentInChildren<Camera>();
        coll.enabled = false;
    }

    public virtual void Unequip()
    {
        coll.enabled = true;
    }

    public virtual void Shoot()
    {
        // You can use this ray for hitscan weapons, if needed
        Ray ray = ownerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 1000f);
        Vector3 direction;
        if (hitInfo.collider != null)
        {
            direction = hitInfo.point - mainMuzzle.transform.position;
        }
        else
        {
            direction = (mainMuzzle.transform.position + ray.direction * 10f) - mainMuzzle.transform.position;
        }
        //GameManager.Instance.GetBall(mainMuzzle.position, direction, damage);
        isShooting = true;
        Invoke("ResetShooting", shotDelay);
    }

    void ResetShooting()
    {
        isShooting = false;
    }
}
