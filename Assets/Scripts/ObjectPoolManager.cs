using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{     
    public GameObject duitPrefab,toyolPrefab,pocongPrefab, hantuRayaPrefab;
    public int totalDuit,totalToyol, totalPocong, totalHantuRaya;
    public List<GameObject> duitList, toyolList, pocongList, hantuRayaList;

    // Start is called before the first frame update

    private void Awake() 
    {
        for(var i = 0 ; i < totalDuit; i++){
            GameObject duit = Instantiate(duitPrefab);
            duit.transform.SetParent(gameObject.transform);
            duit.SetActive(false);
            duitList.Add(duit);
        }
        for(var i = 0 ; i < totalToyol; i++){
            GameObject toyol = Instantiate(toyolPrefab);
            toyol.transform.SetParent(gameObject.transform);
            toyol.SetActive(false);
            toyolList.Add(toyol);
        }
        for(var i = 0 ; i < totalPocong; i++){
            GameObject pocong = Instantiate(pocongPrefab);
            pocong.transform.SetParent(gameObject.transform);
            pocong.SetActive(false);
            pocongList.Add(pocong);
        }
        for(var i = 0 ; i < totalHantuRaya; i++){
            GameObject hantuRaya = Instantiate(hantuRayaPrefab);
            hantuRaya.transform.SetParent(gameObject.transform);
            hantuRaya.SetActive(false);
            hantuRayaList.Add(hantuRaya);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject RequestPrefab(string type){
        if(type == "Duit"){
            foreach (var duit in duitList)
            {
                if(!duit.gameObject.activeSelf){
                    return duit;
                }
            }
            return null;
        }
        if(type == "Toyol"){
            foreach (var toyol in toyolList)
            {
                if(!toyol.gameObject.activeSelf){
                    return toyol;
                }
            }
            return null;
        }
        if(type == "Pocong"){
            foreach (var pocong in pocongList)
            {
                if(!pocong.gameObject.activeSelf){
                    return pocong;
                }
            }
            return null;
        }
        if(type == "HantuRaya"){
            foreach (var hantuRaya in hantuRayaList)
            {
            if(!hantuRaya.gameObject.activeSelf){
                return hantuRaya;
            }else{
            }
            }
            return null;
        }
        return null;
    }
}
