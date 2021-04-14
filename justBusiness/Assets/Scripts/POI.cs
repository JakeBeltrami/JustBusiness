using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : MonoBehaviour
{
    private GameObject[] points;
    private LayerMask layerMask;
    private LayerMask enemyMask;
    private LayerMask obstacleMask;
    private LayerMask lowEnvMask;
    private LayerMask toggleMask;
    private LayerMask poiMask;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public List<GameObject> validPoints;
    private GameManager gameManager;
    private ResetPrompt reset;
    private bool prompted;
    private Dictionary<string, Sprite> typeMap;


    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.FindGameObjectsWithTag("POI Target");
        typeMap = new Dictionary<string, Sprite>
        {
            { "Melee Inactive", Resources.Load<Sprite>("POI/POI") },
            { "Melee Active", Resources.Load<Sprite>("POI/POI Active") },
            { "Shoot Inactive", Resources.Load<Sprite>("POI/Shoot POI Inactive") },
            { "Shoot Active", Resources.Load<Sprite>("POI/Shoot POI Active") },
            { "In Range", Resources.Load<Sprite>("POI/POI In Range") },
        };
        layerMask = LayerMask.GetMask("POI");
        enemyMask = LayerMask.GetMask("Enemy");
        toggleMask = LayerMask.GetMask("Toggle");
        obstacleMask = LayerMask.GetMask("Environment");
        lowEnvMask = LayerMask.GetMask("Low Environment");
        poiMask = LayerMask.GetMask("Toggle");
        validPoints = new List<GameObject>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = gameManager.Player.gameObject;
        // If using reset, use commented code
        reset = null;
        //reset = GameObject.FindGameObjectWithTag("Reset").GetComponent<ResetPrompt>();
        prompted = false;
    }

    private void Update()
    {
        // ValidPOI(player.GetComponent<PlayerInput>().dashRange);
    }

    public GameObject CheckPOIInteraction(float dashRange)
    {
        ValidPOI(dashRange); // Not necessary if called in update
        // If none, prompt reset after delay
        // NOTE: Doesn't factor in waiting for Enemies to come to you
        if (validPoints.Count == 0 && gameManager.Enemies.Count != 0 && reset != null)
        {
            // Disable turn manager? Also add check if Enemies are pathfinding?
            if (!prompted)
            {
                StartCoroutine(reset.Prompt());
                prompted = true;
            }
        }
        foreach (GameObject poi in validPoints)
        {
            RaycastHit2D hit;
            if (gameManager.Controller)
            {
                hit = Physics2D.Linecast(player.transform.position, gameManager.LineEnd.position, layerMask);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.GetRayIntersection(ray, 25f, layerMask);
            }

            if (hit)
            {
                if (poi == hit.collider.gameObject)
                {
                    // Make animation instead? Will also allow removal of loop
                    // If outside melee range
                    if (poi.GetComponent<POIType>().Melee)
                    {
                        SetPOIType(poi, true, true);
                    }
                    else
                    {
                        SetPOIType(poi, true, false);
                    }
                    SetSpriteAlpha(poi, 1f);
                    // Tile display
                    poi.transform.GetChild(0).transform.position = poi.GetComponentInParent<IHasTile>().Tile.Center;
                    poi.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    // Show surrounding POIs
                    ShowNearbyPOI(poi, dashRange);
                    // Return parent object
                    return poi.transform.parent.gameObject;
                }
            }
            else
            {
                if (poi.GetComponent<POIType>().Melee)
                {
                    SetPOIType(poi, false, true);
                }
                else
                {
                    SetPOIType(poi, false, false);
                }
                SetSpriteAlpha(poi, 0.5f);
                poi.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        return null;
    }

    // TEMPORARY, code for checking if we are shooting an enemy, change later
    public GameObject CheckShootTarget()
    {
        foreach (GameObject enemy in gameManager.Enemies)
        {
            RaycastHit2D hit;
            if (gameManager.Controller)
            {
                hit = Physics2D.Linecast(player.transform.position, gameManager.LineEnd.position, enemyMask);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, enemyMask);
            }

            if (hit)
            {
                if (enemy == hit.collider.gameObject)
                {
                    // Checks for obstaacles (problem: not all environment are obstacles)
                    /*Vector3 dirToTarget = (enemy.transform.position - player.transform.position).normalized;
					float dstToTarget = Vector2.Distance(player.transform.position, enemy.transform.position);
					bool obstacle = Physics2D.Raycast(player.transform.position, dirToTarget, dstToTarget, obstacleMask);
					bool obstacleToggle = Physics2D.Raycast(player.transform.position, dirToTarget, dstToTarget, toggleMask);

					if(!obstacle && !obstacleToggle)
					{
                    	return enemy;
					}*/
                    return enemy;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Returns a list of POIs in range of the player
    /// </summary>
    /// <param name="range">How far from the player</param>
    /// <returns></returns>
    public void ValidPOI(float range)
    {
        // Update POI
        points = GameObject.FindGameObjectsWithTag("POI Target");
        // Clear list
        validPoints.Clear();
        foreach (GameObject poi in points)
        {
            // Debug.Log(poi.transform.parent.name);
            // Disable first?
            DisablePOI(poi);
            // Null error occurs once per level, not sure why
            Vector3 poiAsGrid = poi.transform.parent.GetComponent<IHasTile>().Tile.Center;
            // Check if not at players location
            if (poiAsGrid != player.transform.position)
            {
                // If in melee range
                if (Vector2.Distance(player.transform.position, poiAsGrid) <= range)
                {
                    // If at cardinal/diagonal direction from player
                    Vector3 dirToPOI = (poiAsGrid - player.transform.position).normalized;
                    // Debug.Log(poi.transform.parent.name + " angle from player is: " + Vector3.SignedAngle(Vector3.up, dirToPOI, Vector3.forward));
                    if (Vector3.SignedAngle(Vector3.up, dirToPOI, Vector3.forward) % 45 == 0)
                    {
                        CheckPOI(poi, poiAsGrid, true);
                    }
                    else
                    {
                        DisablePOI(poi);
                    }
                }
                else
                {
                    // If an Enemy
                    if (poi.transform.parent.tag == "Enemy")
                    {
                        CheckPOI(poi, poiAsGrid, false);
                    }
                    else
                    {
                        DisablePOI(poi);
                    }
                }
            }
            else
            {
                DisablePOI(poi);
            }
        }
    }

    private void ShowNearbyPOI(GameObject sourcePoi, float range)
    {
        Vector3 source = sourcePoi.GetComponentInParent<IHasTile>().Tile.Center;
        foreach (GameObject poi in points)
        {
            Vector3 poiAsGrid = poi.GetComponentInParent<IHasTile>().Tile.Center;
            if (poiAsGrid != source)
            {
                // If in melee range
                if (Vector2.Distance(source, poiAsGrid) <= range)
                {
                    // If at cardinal/diagonal direction from poi
                    Vector3 dirToPOI = (poiAsGrid - source).normalized;
                    if (Vector3.SignedAngle(Vector3.up, dirToPOI, Vector3.forward) % 45 == 0)
                    {
                        // Check POI
                        CheckNearbyPOI(poi, source, poiAsGrid);
                    }
                }
            }
        }
    }

    private void CheckNearbyPOI(GameObject poi, Vector3 source, Vector3 poiAsGrid)
    {
        Vector3 dirToTarget = (poiAsGrid - source).normalized;
        float dstToTarget = Vector2.Distance(source, poiAsGrid);
        RaycastHit2D[] enemyHit = Physics2D.RaycastAll(source, dirToTarget, dstToTarget, enemyMask); // Will negate poi if also enemy though
        bool obstacle = Physics2D.Raycast(source, dirToTarget, dstToTarget, obstacleMask + lowEnvMask);
        // bool lowEnv = Physics2D.Raycast(source, dirToTarget, dstToTarget, lowEnvMask);

        // If no obstacles
        if (!obstacle)
        {
            // Show the nearby POI
            // Debug.Log(poi.transform.parent.name + " is nearby");
            if (enemyHit.Length != 0)
            {
                foreach (RaycastHit2D enemy in enemyHit)
                {
                    if ((Vector3)enemy.transform.GetComponent<IHasTile>().Tile.Center == poiAsGrid && poi.transform.parent.tag == "Enemy")
                    {
                        ShowPOI(poi);
                    }
                }
            }
            else
            {
                ShowPOI(poi);
            }
        }
    }

    private void CheckPOI(GameObject poi, Vector3 poiAsGrid, bool canMelee)
    {
        Vector3 dirToTarget = (poiAsGrid - player.transform.position).normalized;
        float dstToTarget = Vector2.Distance(player.transform.position, poiAsGrid);
        RaycastHit2D[] enemyHit = Physics2D.RaycastAll(player.transform.position, dirToTarget, dstToTarget, enemyMask); // Will negate poi if also enemy though
        bool obstacle = Physics2D.Raycast(player.transform.position, dirToTarget, dstToTarget, obstacleMask);
        bool lowEnv = Physics2D.Raycast(player.transform.position, dirToTarget, dstToTarget, lowEnvMask);

        BaseAttack w = gameManager.Player.MyInput.Inventory.Weapon;
        bool weapon = (w != null && w.Ammo > 0) ? true : false;
        // If no obstacles
        if (!obstacle && !lowEnv)
        {
            // If no enemy
            if (enemyHit.Length == 0)
            {
                if (canMelee)
                {
                    // Add to list of valid POIs
                    EnablePOI(poi, true);
                    validPoints.Add(poi);
                }
                else if (weapon)
                {
                    EnablePOI(poi, false);
                    validPoints.Add(poi);
                }
            }
            // If hit self
            else
            {
                foreach (RaycastHit2D enemy in enemyHit)
                {
                    // If on top of poi and POI is an Enemy
                    if ((Vector3)enemy.transform.GetComponent<IHasTile>().Tile.Center == poiAsGrid && poi.transform.parent.tag == "Enemy")
                    {
                        if (canMelee)
                        {
                            // Add to list of valid POIs
                            EnablePOI(poi, true);
                            validPoints.Add(poi);
                            break;
                        }
                        else if (weapon)
                        {
                            EnablePOI(poi, false);
                            validPoints.Add(poi);
                            break;
                        }
                    }
                    else
                    {
                        DisablePOI(poi);
                    }
                }
            }
        }
        // If only low environment in the way
        else if (!obstacle && lowEnv)
        {
            // If the player has a weapon
            if (weapon)
            {
                foreach (RaycastHit2D enemy in enemyHit)
                {
                    if ((Vector3)enemy.transform.GetComponent<IHasTile>().Tile.Center == poiAsGrid && poi.transform.parent.tag == "Enemy")
                    {
                        EnablePOI(poi, false);
                        validPoints.Add(poi);
                        break;
                    }
                    else
                    {
                        DisablePOI(poi);
                    }
                }
            }
            else
            {
                DisablePOI(poi);
            }
        }
        else
        {
            DisablePOI(poi);
        }
    }

    private void SetPOIType(GameObject poi, bool selected, bool inMeleeRange)
    {
        if (inMeleeRange)
        {
            if (selected)
            {
                poi.GetComponent<SpriteRenderer>().sprite = typeMap["Melee Active"];
            }
            else
            {
                poi.GetComponent<SpriteRenderer>().sprite = typeMap["Melee Inactive"];
            }
        }
        else
        {
            if (selected)
            {
                poi.GetComponent<SpriteRenderer>().sprite = typeMap["Shoot Active"];
            }
            else
            {
                poi.GetComponent<SpriteRenderer>().sprite = typeMap["Shoot Inactive"];
            }
        }
    }

    private void SetSpriteAlpha(GameObject poi, float alpha)
    {
        Color temp = poi.GetComponent<SpriteRenderer>().color;
        temp.a = alpha;
        poi.GetComponent<SpriteRenderer>().color = temp;
    }

    private void EnablePOI(GameObject poi, bool canMelee)
    {
        poi.GetComponent<SpriteRenderer>().enabled = true;
        poi.GetComponent<CircleCollider2D>().enabled = true;
        POIType type = poi.GetComponent<POIType>();
        if (canMelee)
        {
            type.Melee = true;
        }
        else
        {
            type.Melee = false;
        }
        type.Selectable = true;
    }

    private void DisablePOI(GameObject poi)
    {
        poi.GetComponent<SpriteRenderer>().enabled = false;
        poi.GetComponent<CircleCollider2D>().enabled = false;
        poi.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        poi.GetComponent<POIType>().Melee = false;
        SetPOIType(poi, false, true);
    }

    public void DisableAllPOI()
    {
        foreach (GameObject poi in points)
        {
            if (poi != null)
            {
                DisablePOI(poi);
            }
        }
        // validPoints.Clear();
    }

    /// <summary>
    /// Show a POI in range of another POI
    /// </summary>
    private void ShowPOI(GameObject poi)
    {
        poi.GetComponent<POIType>().Selectable = false;
        poi.GetComponent<SpriteRenderer>().enabled = true;
        poi.GetComponent<SpriteRenderer>().sprite = typeMap["In Range"];
        SetSpriteAlpha(poi, 0.5f);
        poi.transform.GetChild(0).transform.position = poi.GetComponentInParent<IHasTile>().Tile.Center;
        poi.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }
}
