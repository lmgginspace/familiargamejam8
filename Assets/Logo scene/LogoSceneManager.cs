using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoSceneManager : MonoBehaviour
{
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Atributos
    // ---- ---- ---- ---- ---- ---- ---- ----
    [Header("Stars")]
    public ParticleSystem starParticleSystem;

    [Header("Animation curves")]
    public AnimationCurve directionalLightAngleCurve;
    public AnimationCurve cameraXCurve;
    public AnimationCurve cameraYCurve;
    public AnimationCurve cameraZCurve;
    public AnimationCurve pointLightXCurve;
    public AnimationCurve screenFadeOutAlphaCurve;
    public AnimationCurve sunObjectScaleCurve;

    [Header("Animation objects")]
    public Transform directionalLight;
    public Transform mainCamera;
    public Transform pointLight;
    public Transform sunObject;
    public Image screenFadeOutImage;

    [Header("Audio")]
    public AudioSource logoMusicSource;

    [Header("Length")]
    public float logoStayTime = 10.0f;

    // Atributos privados
    private bool alive;
    private bool ableToSkip;

    // ---- ---- ---- ---- ---- ---- ---- ----
    // Métodos
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Métodos de MonoBehaviour
    private void Start()
    {
        this.alive = true;
        this.ableToSkip = false;

        this.starParticleSystem.randomSeed = 113;
        this.starParticleSystem.Simulate(0.0f, true, true);
        this.starParticleSystem.Play();

        this.StartCoroutine(this.AnimateObjects());
        this.StartCoroutine(this.TurnSkipOn(0.25f));

        this.logoMusicSource.PlayDelayed(0.5f);
    }

    private void OnDestroy()
    {
        this.alive = false;
    }

    private void Update()
    {
        if ((Time.timeSinceLevelLoad > this.logoStayTime) || (this.ableToSkip && Input.anyKeyDown))
            this.LoadNextScene();
    }

    // Métodos auxiliares
    private void LoadNextScene()
    {
        Scene nextScene, currentScene = SceneManager.GetActiveScene();
        int nextSceneIndex = currentScene.buildIndex + 1;

        try
        {
            nextScene = SceneManager.GetSceneAt(nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
        }
        catch (Exception e)
        {
            Debug.LogWarningFormat("Unable to load next scene. Error: {0}", e.Message);
        }
    }

    // Corrutinas
    private IEnumerator AnimateObjects()
    {
        float pointLightY = this.pointLight.localPosition.y;
        float pointLightZ = this.pointLight.localPosition.z;

        float time = 0.0f;
        while (this.alive)
        {
            time += Time.deltaTime;

            // Luz direccional
            this.directionalLight.localRotation = Quaternion.Euler(0.0f, this.directionalLightAngleCurve.Evaluate(time), 0.0f);

            // Cámara
            this.mainCamera.localPosition = new Vector3(this.cameraXCurve.Evaluate(time),
                this.cameraYCurve.Evaluate(time),
                this.cameraZCurve.Evaluate(time));

            // Luz puntual
            this.pointLight.localPosition = new Vector3(this.pointLightXCurve.Evaluate(time), pointLightY, pointLightZ);

            // Sol
            this.sunObject.localScale = Vector3.one * this.sunObjectScaleCurve.Evaluate(time);

            // Image de fading
            this.screenFadeOutImage.color = new Color(0.0f, 0.0f, 0.0f, this.screenFadeOutAlphaCurve.Evaluate(time));

            yield return null;
        }
    }

    private IEnumerator TurnSkipOn(float afterTime)
    {
        float time = 0.0f;
        while (time < afterTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        this.ableToSkip = true;
    }

}