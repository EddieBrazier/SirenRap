﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    //list of beats used for the song and the initial beat number
    [Tooltip("These are the beats for normal mode, with beat type respresnting the input the player must make to hit the beat")]
    public List<BeatType> beatList;
    public int beatNumber;

    //music beat prefab to spawn
    public GameObject musicBeat;

    //list of beat times
    [Tooltip("This is the time at which each beat appears, with the float representing the time in seconds into the game that the beat will spawn")]
    public List<float> timeList;

    //float for tracking the next time to spawn a beat
    private float nextSpawnTime;

    //transform for beat spawn
    public Transform beatSpawn;

    //variable for beat timing
    public float beatSpeed;

    //hitbox object
    public GameObject hitBox;

    //canvas object
    [SerializeField]
    private Canvas gameUI;

    //the audio source for the level's song
    private AudioSource levelSong;

    //float for tracking level time
    private float levelTime;

    //bool to check if the level was started
    public bool levelStart = false;

    //level end UI gameobject
    public GameObject endUI;

    // Start is called before the first frame update
    void Start()
    {
        //save the level song
        levelSong = GetComponent<AudioSource>();

        //save beat number
        beatNumber = beatList.Count;

        //set the first spawn time
        nextSpawnTime = timeList[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //spawn notes when their time comes up
        if (levelTime >= nextSpawnTime)
        {
            //spawn new beat
            GameObject newBeat = Instantiate(musicBeat, beatSpawn.position, Quaternion.identity);
            //GameObject newBeat = Instantiate(musicBeat, beatSpawn.position, Quaternion.identity, gameUI.transform);

            //set beat type and move increment
            newBeat.GetComponent<MusicBeat>().SetBeatType(beatList[0]);
            newBeat.GetComponent<MusicBeat>().SetMoveIncrement(hitBox.transform.position.x, beatSpeed);

            //set beat list and next beat time values
            beatList.RemoveAt(0);
            timeList.RemoveAt(0);

            if(timeList.Count != 0)
            {
                nextSpawnTime = timeList[0];
            }
            else
            {
                //reset level time
                levelTime = 0f;
            }
        }

        //end the level if the song isn't playing and the level was started
        if (levelSong.isPlaying != true && levelStart == true)
        {
            //reset background
            Camera.main.GetComponent<BackgroundPulse>().ResetColor();

            //activate end UI
            endUI.SetActive(true);
        }

        //increase level time if there are more notes to spawn
        if (beatList.Count > 0 && levelSong.isPlaying == true)
        {
            levelTime += Time.deltaTime;
        }
    }
}
