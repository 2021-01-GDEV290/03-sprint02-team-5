using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TierManager : MonoBehaviour
{
    private PlayerController _player;

    [Header("Dot Manager")]
    public GameObject healthTiers;
    private GameObject _healthTier1;
    private GameObject _healthTier2;
    private GameObject _healthTier3;

    public GameObject attackTiers;
    private GameObject _attackTier1;
    private GameObject _attackTier2;
    private GameObject _attackTier3;

    public GameObject regenTiers;
    private GameObject _regenTier1;
    private GameObject _regenTier2;
    private GameObject _regenTier3;

    [Header("Cost Manager")]
    public Text healthCost;
    public Text attackCost;
    public Text regenCost;

    [Header("Button Manager")]
    public Button healthButton;
    public Button attackButton;
    public Button regenButton;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _healthTier1 = healthTiers.transform.GetChild(0).gameObject;
        _healthTier2 = healthTiers.transform.GetChild(1).gameObject;
        _healthTier3 = healthTiers.transform.GetChild(2).gameObject;

        _attackTier1 = attackTiers.transform.GetChild(0).gameObject;
        _attackTier2 = attackTiers.transform.GetChild(1).gameObject;
        _attackTier3 = attackTiers.transform.GetChild(2).gameObject;

        _regenTier1 = regenTiers.transform.GetChild(0).gameObject;
        _regenTier2 = regenTiers.transform.GetChild(1).gameObject;
        _regenTier3 = regenTiers.transform.GetChild(2).gameObject;

        InitializeCost();
        RefreshTiers();
    }

    private void InitializeCost()
    {
        healthCost.text = _player.tier1cost.ToString();
        attackCost.text = _player.tier1cost.ToString();
        regenCost.text = _player.tier1cost.ToString();
    }

    public void RefreshTiers()
    {
        if (_player.healthUpgrade >= 3)
        {
            _healthTier3.GetComponent<Image>().color = Color.green;
            healthCost.text = "MAX";
            healthButton.interactable = false;
        }
        else if (_player.healthUpgrade == 2)
        {
            _healthTier2.GetComponent<Image>().color = Color.green;
            healthCost.text = _player.tier3cost.ToString();

            if (_player.CanAfford(3)) healthButton.interactable = true;
            else healthButton.interactable = false;
        }
        else if (_player.healthUpgrade == 1)
        {
            _healthTier1.GetComponent<Image>().color = Color.green;
            healthCost.text = _player.tier2cost.ToString();

            if (_player.CanAfford(2)) healthButton.interactable = true;
            else healthButton.interactable = false;
        }
        else
        {
            if (_player.CanAfford(1)) healthButton.interactable = true;
            else healthButton.interactable = false;
        }

        if (_player.attackUpgrade >= 3)
        {
            _attackTier3.GetComponent<Image>().color = Color.green;
            attackCost.text = "MAX";
            attackButton.interactable = false;
        }
        else if (_player.attackUpgrade == 2)
        {
            _attackTier2.GetComponent<Image>().color = Color.green;
            attackCost.text = _player.tier3cost.ToString();

            if (_player.CanAfford(3)) attackButton.interactable = true;
            else attackButton.interactable = false;
        }
        else if (_player.attackUpgrade == 1)
        {
            _attackTier1.GetComponent<Image>().color = Color.green;
            attackCost.text = _player.tier2cost.ToString();

            if (_player.CanAfford(2)) attackButton.interactable = true;
            else attackButton.interactable = false;
        }
        else
        {
            if (_player.CanAfford(1)) attackButton.interactable = true;
            else attackButton.interactable = false;
        }
                
        if (_player.regenUpgrade >= 3)
        {
            _regenTier3.GetComponent<Image>().color = Color.green;
            regenCost.text = "MAX";
            regenButton.interactable = false;
        }
        else if (_player.regenUpgrade == 2)
        {
            _regenTier2.GetComponent<Image>().color = Color.green;
            regenCost.text = _player.tier3cost.ToString();

            if (_player.CanAfford(3)) regenButton.interactable = true;
            else regenButton.interactable = false;
        }
        else if (_player.regenUpgrade == 1)
        {
            _regenTier1.GetComponent<Image>().color = Color.green;
            regenCost.text = _player.tier2cost.ToString();

            if (_player.CanAfford(2)) regenButton.interactable = true;
            else regenButton.interactable = false;
        }
        else
        {
            if (_player.CanAfford(1)) regenButton.interactable = true;
            else regenButton.interactable = false;
        }
    }    
}
