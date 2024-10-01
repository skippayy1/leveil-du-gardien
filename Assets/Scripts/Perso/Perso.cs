using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

/// <summary>
/// Classe qui controle les déplacements du personnage
/// Hérite de la classe de détection de collision pour vérifier la collision avec le sol
/// Auteurs du code: Luka Munger et Victor Aubry
/// Auteur des commentaires: Victor Aubry
/// </summary>
public class Perso : DetectionCollisions
{
    [Header("Sons")]
    [SerializeField] AudioClip _sonSaut; // #tp4 Victor Son du saut
    [SerializeField] AudioClip _sonAtterisage; // #tp4 Victor Son de l'atterissage

    [Header("Paramètres du perso")]
    [SerializeField] float _vitesseIni = 5f; // #synthese luka Vitesse du déplacement horizontal
    float _vitesseActuelle; // #synthese luka Vitesse actuelle du déplacement horizontal
    [SerializeField] float _forceSaut = 50f; //Force appliquée à toutes les frames du saut
    [SerializeField] int _dureeMaxSaut = 10; //Durée max du saut (en frame)
    [SerializeField] float _forceEsquive = 10f; // #tp3 luka force de la esquive horizontale du personnage #tp3 luka
    [SerializeField] float _dureeEsquive = 1.0f; // #tp3 luka duree de la esquive du personnage #tp3 luka
    [SerializeField] float _dureeTempsAttenteEsquive = 5.0f; // #tp3 luka duree de temps que le joueur ne peut pas esquiver entre chaque esquive
    [SerializeField] IconesObjets _iconesObjets; // #tp3 luka 
    [SerializeField] SOPerso _donneesPerso; // #tp3 luka acces au scriptable object du personnage

    [Header("Bonus aimant")]
    [SerializeField] float _distanceAttraction; // #tp3 luka distance d'attraction d'objets de l'aimant
    [SerializeField] float _forceAttraction; // #tp3 luka force d'attraction de l'aimant
    [SerializeField] LayerMask _masqueCalquesAimante; // #tp3 luka masque de calques pour detecter les objets
    [SerializeField] float _dureeBonusAimant; // #tp3 luka durée de l'attraction de l'aimant
    [SerializeField] GameObject _particulesAimant; // #tp3 Victor Modèle du système de particules d'aimant
    bool _aimantEstActive = false;

    [Header("Bonus vitesse")]
    [SerializeField] float _dureeBonusVitesse = 5f; // #tp3 Victor Durée du bonus de vitesse
    [SerializeField] float _multiplicateurBonusVitesse = 1.5f; // #tp3 Victor Multiplicateur bonus de la vitesse
    [SerializeField] GameObject _particulesVitesse; // #tp3 Victor Modèle du système de particules de vitesse
    bool _bonusVitesseActive = false;

    [Header("Block/Parry")]
    [SerializeField] float _dureeParry = 0.5f; // #synthese luka duree du parry
    bool _estEnEsquive = false; // #tp3 luka verifie si le personnage est en esquive #tp3 luka
    bool _estEnBlock = false; // #synthese luka verifie si le personnage est en block
    bool _estEnParry = false; // #synthese luka verifie si le personnage est en parry
    public bool estEnParry => _estEnParry; // #synthese luka getter de estEnParry

    #region #synthese Victor
    [Header("Attaque")]
    [SerializeField] float _rayonAttaque = 2f; // Rayon du cercle d'attaque
    [SerializeField] Vector2 _decalageAttaque = new(1.5f, 1f); // Décalage du cercle d'attaque
    [SerializeField] float _forcePropulsion = 2f; // Force de propulsion de l'attaque
    [SerializeField] float _soulevement = 0.5f; // À quel point l'ennemi est soulevé lorqu'il est frappé
    [SerializeField] AudioClip _sonAttaque; // Son de l'attaque
    [SerializeField] LayerMask _masqueEnnemis; // Masque des ennemis pour la collision
    #endregion

    bool _doubleSautEstAchete = false; // #tp3 luka verifie si le pero possede l'objet pour double sauter
    bool _peutDoubleSaut; //Détermine si le joueur peut sauter à nouveau
    bool _sautAppuye = false; //Détermine si la touche de saut est enfoncée
    int _dureeRestantSaut = 0; //Frame restant au saut
    bool _etaitEnCollision; // #tp4 Victor Permet de savoir si le personnage était en collision à la frame précédente
    float _axeHorizontal; //Valeur de la saisie pour le déplacement horizontal
    Rigidbody2D _rb; //RigidBody
    SpriteRenderer _sr; //SpriteRenderer
    Animator _animator; // #tp4 Victor Animator du personnage
    bool _peutEsquiver = true; // #tp3 luka verifie si le personnage peut esquiver
    float _multiplicateurVitesse = 1; // #tp3 Victor Multiplie la vitesse

    #region #tp3 Victor
    [Header("Modification des particules")]
    [SerializeField] int _rateOverDistanceBase; // #tp3 Victor Valeur de base du Rate over Distance
    ParticleSystem[] _particulesCourse; // #tp3 Victor Particules de la course
    #endregion
    public bool estEnBlock => _estEnBlock;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>(); // #tp4 Victor
        _peutEsquiver = _donneesPerso.peutEsquiver;
        _doubleSautEstAchete = _donneesPerso.peutDoubleSaut;
        _particulesCourse = GetComponentsInChildren<ParticleSystem>();
        _vitesseActuelle = _vitesseIni;
        _donneesPerso.perso = this;
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_estEnEsquive) // #tp3 luka permet a la coroutine de esquive de faire son esquive sans avoir une réinitialisation de la vélocité en x
        {
            //Change la vélocité horizontal en fonction de la saisie et de la vitesse:
            _rb.velocity = new(_vitesseActuelle * _axeHorizontal * _multiplicateurVitesse, _rb.velocity.y);

            if (_sautAppuye)
            {
                //Tant que la touche de saut est appuyé et qu'il reste des frames
                //de saut, la force du saut est appliqué (de moins en moins fort):
                float fractionForce = (float)_dureeRestantSaut / _dureeMaxSaut;
                _rb.AddForce(_forceSaut * fractionForce * Vector2.up);
                if (_dureeRestantSaut > 0) _dureeRestantSaut--;
            }
            else if (_estEnCollision)
            {
                _dureeRestantSaut = _dureeMaxSaut;
                if (_doubleSautEstAchete) // #tp3 luka verifie si le perso est en collission et le double saut est achete pour faire le double saut
                {
                    _peutDoubleSaut = true;
                }
            }
            else _dureeRestantSaut = 0;
        }

        if (_aimantEstActive) Aimanter();

        #region #tp3 Victor
        foreach (ParticleSystem particleSystem in _particulesCourse)
        {
            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            emissionModule.rateOverDistance = _estEnCollision ? _rateOverDistanceBase : 0;
        }
        #endregion

        #region #tp4 Victor
        // Donne la vélocité normalisée à l'animateur
        Vector2 velociteNormalise = _rb.velocity.normalized;
        _animator.SetFloat("VitesseX", Mathf.Abs(velociteNormalise.x));
        _animator.SetFloat("VitesseY", velociteNormalise.y);

        // Joue le son d'atterisage si le personnage est en collision et qu'il n'était pas en collision à la frame précédente
        if (_estEnCollision != _etaitEnCollision && _estEnCollision) GestSons.instance.JouerSon(_sonAtterisage, 1f);
        _etaitEnCollision = _estEnCollision;
        #endregion
    }

    /// <summary>
    /// Fonction appelée lorsque la valeur de la saisie
    /// pour le déplacement horizontal change.
    /// </summary>
    /// <param name="value">Valeur de la saisie</param>
    void OnMove(InputValue value)
    {
        //Obtention de la valeur de la saisie:
        _axeHorizontal = value.Get<float>();
        //Tourne le perso pour qu'il regarde vers où il va:
        if (_axeHorizontal < 0) _sr.flipX = true;
        else if (_axeHorizontal > 0) _sr.flipX = false;
    }

    /// <summary>
    /// Fonction appelée lorsque la valeur de
    /// la saisie pour le saut change.
    /// </summary>
    /// <param name="value">Valeur de la saisie</param>
    void OnJump(InputValue value)
    {
        _sautAppuye = value.isPressed;
        if (_sautAppuye) SautAppuye();
    }

    void OnApplicationQuit() // #tp3 luka cette methode est utilisee pour reinitialiser les valeurs du personnage lorsquon quitte le jeu
    {
        _donneesPerso.InitialiserNiveau(); // #tp3 luka reinitialise les donnees du personnage
    }

    void OnDestroy()
    {
        _donneesPerso.InitialiserNiveau(); // #tp3 luka reinitialise les donnees du personnage
    }

    /// <summary>
    /// #tp3 luka 
    /// Fonction appelée lorsque la valeur de la esquive est appuyée.
    /// </summary>
    /// <param name="value">Valeur de la saisie</param>
    void OnDash(InputValue value)
    {
        if (_peutEsquiver == true)
        {
            Coroutine coroutineEsquive = StartCoroutine(CoroutineEsquive());
            Coroutine coroutineTempsAttenteEsquive = StartCoroutine(CoroutineTempsAttenteEsquive());
        }
    }

    /// <summary>
    /// #tp3 luka
    /// Cette coroutine permet de controler la velocite et la gravite lors de 
    /// esquivation du personnage
    /// </summary>
    /// <returns>Duree de l'esquive</returns>
    IEnumerator CoroutineEsquive()
    {
        _estEnEsquive = true;

        Vector2 _directionEsquive = _sr.flipX ? Vector2.left : Vector2.right; // #tp3 luka verifie la direction du perso pour faire la esquive dans la bonne direction

        _rb.velocity = _directionEsquive * _forceEsquive;
        _rb.gravityScale = 0.5f; // #tp3 luka reduction de la gravite pour un effet plus agreable

        yield return new WaitForSeconds(_dureeEsquive); // #tp3 luka duree de temps que la coroutine laisse la esquive aller avant de ramener le personnage a sa vitesse initiale
        _rb.gravityScale = 3f;
        _rb.velocity = new Vector2(_vitesseActuelle * _axeHorizontal, _rb.velocity.y);
        _estEnEsquive = false;
    }
    /// <summary>
    /// #tp3 luka
    /// cette coroutine permet de partir le temps d'attente entre chaque esquive
    /// </summary>
    IEnumerator CoroutineTempsAttenteEsquive()
    {
        _peutEsquiver = false;
        // _iconesObjets.DemarrerCoroutAttente(_dureeTempsAttenteEsquive);
        yield return new WaitForSeconds(_dureeTempsAttenteEsquive);
        _peutEsquiver = true;
    }

    /// <summary>
    /// Fonction appelée lorque la touche pour le saut est appuyée
    /// </summary>
    void SautAppuye()
    {
        if (_peutDoubleSaut && !_estEnCollision)
        {
            _dureeRestantSaut = _dureeMaxSaut;
            _rb.velocity = new(_rb.velocity.x, 0);
            _peutDoubleSaut = false;
        }
        if (_estEnCollision) GestSons.instance.JouerSon(_sonSaut, 1f); // #tp4 Victor Joue le son du saut
    }

    /// <summary>
    /// #tp2 luka
    /// cette methode permet d'activer la fonction de l'aimant, et d'attirer les objets vers le joueur
    /// </summary>
    public void ActiverAimant()
    {
        StartCoroutine(CouroutineBonusAimant());
    }

    IEnumerator CouroutineBonusAimant()
    {
        Instantiate(_particulesAimant, transform); // #tp3 Victor Instanciation du système de particules
        _aimantEstActive = true;
        yield return new WaitForSeconds(_dureeBonusAimant);
        _aimantEstActive = false;
    }

    /// <summary>
    /// #tp3 luka
    /// cette methode permet de gerer les fonctions de l'aimant et d'attirer 
    /// </summary>
    void Aimanter()
    {
        Collider2D[] objetsAimante = Physics2D.OverlapCircleAll(transform.position, _distanceAttraction, _masqueCalquesAimante); // cree un overlap circle autour du personnage pour detecter les joyaux
        foreach (Collider2D objetAimante in objetsAimante)
        {
            if (objetAimante.gameObject != gameObject)
            {
                Rigidbody2D rb = objetAimante.attachedRigidbody;
                if (rb != null)
                {
                    Vector2 direction = (transform.position - objetAimante.transform.position).normalized;
                    rb.AddForce(direction * _forceAttraction, ForceMode2D.Force);
                    Debug.Log(direction * _forceAttraction);
                    // ajoute la force sur l'objet a attirer
                }
            }
        }
    }

    /// <summary>
    /// #tp2 Victor
    /// Activer et désactive le bonus de course qui fait déplacer le joueur plus vite
    /// </summary>
    public void ActiverBonusCourse()
    {
        if (!_bonusVitesseActive) StartCoroutine(CouroutineBonusCourse());
    }

    IEnumerator CouroutineBonusCourse()
    {
        Instantiate(_particulesVitesse, transform); // #tp3 Victor Instanciation du système de particules
        float multiplicateurVitesseBase = _multiplicateurVitesse;
        _multiplicateurVitesse = _multiplicateurBonusVitesse;
        _bonusVitesseActive = true;
        yield return new WaitForSeconds(_dureeBonusVitesse);
        _multiplicateurVitesse = multiplicateurVitesseBase;
        _bonusVitesseActive = false;
    }

    void OnBlock(InputValue value)
    {
        if (_estEnCollision)
        {
            _estEnBlock = value.isPressed;
            _animator.SetBool("estEnBlock", _estEnBlock);
            if (_estEnBlock)
            {
                _vitesseActuelle = 0;
                StartCoroutine(CoroutParry());
            }
            else _vitesseActuelle = _vitesseIni;
        }

    }

    IEnumerator CoroutParry()
    {
        _estEnParry = true;
        yield return new WaitForSeconds(_dureeParry);
        _estEnParry = false;
    }

    void OnAttack()
    {
        _animator.SetTrigger("Attaque");
        GestSons.instance.JouerSon(_sonAttaque, 1f);

        // Si l'ennemi est dans le rayon d'attaque du joueur, il est projeté
        Vector3 posColl = transform.position + Vector3.Reflect((Vector3)_decalageAttaque, _sr.flipX? Vector3.right : Vector3.zero);
        Collider2D coll = Physics2D.OverlapCircle(posColl, _rayonAttaque, _masqueEnnemis);
        Vector3 force = new Vector3(_sr.flipX? -1 : 1, _soulevement, 0) * _forcePropulsion;
        if (coll != null) StartCoroutine(coll.GetComponentInParent<EnnemiBase>().Projeter(force));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.Reflect((Vector3)_decalageAttaque, Vector3.zero), _rayonAttaque);

    }
}