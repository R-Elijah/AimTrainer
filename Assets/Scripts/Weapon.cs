using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;

    private Camera mainCam;
    private AudioSource shotSound;
    private float targetMoveSpeed = 1;
    private bool inPractice = false;

    public Text hitsDisplay;
    public Text accDisplay;
    public Text pracDisplay;

    void Awake()
    {
        //track main camera in variable
        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        shotSound = GetComponents<AudioSource>()[0];
        ResetStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
			CastRay();
            HandleGunShot();
		} else if (Input.GetButtonDown("Practice")) {
            PracticeMode();
        }
    }

    // resets the trackers and text diplays
    private void ResetStats()
    {
        PlayerStats.hitsTracker = PlayerStats.shotsTracker = 0;
        hitsDisplay.text = "Hits: " + PlayerStats.hitsTracker;
        accDisplay.text = "Accuracy: 0.00%";
    }

    private void CastRay()
    {
        // struct object that will hold our raycast information
        RaycastHit hit;

        // if we collide with a target with our raycast, trigger its hit function
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, 20))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                // update tracker stats
                PlayerStats.hitsTracker += 1;
                hitsDisplay.text = "Hits: " + PlayerStats.hitsTracker;
                if (!inPractice)
                {
                    // move the target back and forth
                    StartCoroutine(TargetMoveCoroutine(hit.collider.gameObject, 1));
                }
            }
        }
        // update tracker stats
        PlayerStats.shotsTracker += 1;
        accDisplay.text = "Accuracy: " + string.Format("{0:0.##}", ((double)PlayerStats.hitsTracker / PlayerStats.shotsTracker) * 100) + "%";
    }

    // moves the passed object on the z-axis and then back after 1 sec
    // units moved are 1 and direction is determined by the dir int
    // 1 for +z and -1 for -z
    IEnumerator TargetMoveCoroutine(GameObject obj, int dir)
    {
        // move to new position
        obj.transform.position += new Vector3(0, 0, 1 * dir) * targetMoveSpeed;
        yield return new WaitForSeconds(1);
        // move back
        obj.transform.position += new Vector3(0, 0, -1 * dir) * targetMoveSpeed;
    }

    // plays the muzzle flash and shot sound
    private void HandleGunShot()
    {
        shotSound.Play();
        if (muzzleFlash.isPlaying)
            muzzleFlash.Stop();
        muzzleFlash.Play();
    }

    // practice mode setup
    private void PracticeMode()
    {
        GameObject[] targets;
        pracDisplay.text = "Hit the targets as many times as possible.";
        inPractice = true;

        // reset trackers
        ResetStats();
        // get all the targets
        targets = GameObject.FindGameObjectsWithTag("Target");
        // move them all out of the range
        foreach (GameObject target in targets)
        {
            target.transform.position += Vector3.forward * targetMoveSpeed;
        }
        // start the actual practice script
        StartCoroutine(PracticeCoroutine(targets));
    }

    // moves random targets into position and out over the course of about 60 seconds
    IEnumerator PracticeCoroutine(GameObject[] targets)
    {
        // rough seconds tracker to spawn about 60 targets over 60 seconds
        int secs = 0;
        // old index tracker to avoid choosing the same target twice in a row
        int oldIndex = -1;
        while (secs < 60)
        {
            // get a target between 1 and 15
            int index = Random.Range(0, 15);
            if (index != oldIndex)
            {
                // set oldIndex to index for next loop
                oldIndex = index;
                StartCoroutine(TargetMoveCoroutine(targets[index], -1));
                yield return new WaitForSeconds(1);
                secs++;
            }
        }
        inPractice = false;
        yield return new WaitForSeconds(1);
        // go to results
        SceneManager.LoadScene("Results");
    }
}
