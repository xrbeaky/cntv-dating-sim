using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeParser : MonoBehaviour
{
    [SerializeField] DialogueGraph graph;
    [SerializeField] float typingSpeed = 0.05f;
    Coroutine _parser;

    int choiceIndex = -1;

    [SerializeField] TextMeshProUGUI speakerNameText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image speakerImage;
    [SerializeField] Image backgroundImage;
    [SerializeField] Transform choicePrefab;
    [SerializeField] Transform choiceContainer;
    [SerializeField] GameObject choiceParent;
    [SerializeField] GameObject nameParent;
    [SerializeField] GameObject dialogParent;
    [SerializeField] Animator anim;
    [SerializeField] AudioManager am;

    string currentText;
    bool isTyping = false;

    private void Awake()
    {
        ChoiceButton.DialogueChoice += ReceiveIndex;
    }

    private void Start()
    {
        choiceParent.gameObject.SetActive(false);

        foreach(BaseNode b in graph.nodes)
        {
            if(b.GetString() == "Start")
            {
                graph.current = b;
                break;
            }
        }
        _parser = StartCoroutine(ParseNode());
    }

    void Update(){
        if(isTyping && Input.GetKeyDown(KeyCode.Return)){
            dialogueText.text = currentText;
            isTyping = false;
            StopCoroutine(TypeText(currentText));
        }
    }

    IEnumerator ParseNode()
    {
        BaseNode b = graph.current;
        string data = b.GetString();
        string[] dataParts = data.Split('/');

        switch (dataParts[0])
        {
            case "Start":
                NextNode("exit");
                break;
            case "DialogueNode":
                dialogParent.SetActive(true);
                speakerImage.gameObject.SetActive(true);
                nameParent.SetActive(true);
                if(dataParts[1] != ""){
                    speakerNameText.text = dataParts[1];
                }
                else{
                    nameParent.SetActive(false);
                }
                currentText = dataParts[2];
                StartCoroutine(TypeText(dataParts[2]));
                if(b.GetSprite() != null){
                    speakerImage.sprite = b.GetSprite(); 
                }
                else{
                    speakerImage.gameObject.SetActive(false);
                }
                
                backgroundImage.sprite = b.GetBackground();

                yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !isTyping); 
                NextNode("exit");
                break;
            case "ChoiceNode":
                dialogParent.SetActive(true);
                speakerImage.gameObject.SetActive(true);
                Choice[] choices = b.GetChoices();

                ClearChoices();
                for(int i = 0; i < choices.Length; i++)
                {
                    choiceParent.gameObject.SetActive(true);

                    var choice = Instantiate(choicePrefab, choiceContainer.transform, false);
                    choice.GetChild(0).GetComponent<TextMeshProUGUI>().text = choices[i].shortHand;
                    choice.GetComponent<ChoiceButton>().SetIndex(i);
                }

                yield return new WaitUntil(() => choiceIndex >= 0);

                choiceParent.gameObject.SetActive(false);
                NextNode("exit " + choiceIndex);

                break;
            case "BlankNode":
                dialogParent.SetActive(false);
                speakerImage.gameObject.SetActive(false);
                backgroundImage.sprite = b.GetBackground();
                yield return new WaitForSeconds(2f);
                NextNode("exit");
                break;
            case "FadeNode":
                anim.SetTrigger("transition");
                yield return new WaitForSeconds(1.1f);
                NextNode("exit");
                break;
            case "AudioNode":
                am.PlaySound(b.GetSoundName());
                NextNode("exit");
                break;
            case "Exit":
                NextGraph();
                break;
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        string currentText = "";
        for (int i = 0; i <= text.Length && isTyping; i++)
        {
            currentText = text.Substring(0, i);
            dialogueText.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void NextNode(string fieldName)
    {
        choiceIndex = -1;
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }

        graph.current = graph.current.GetOutputPort(fieldName).Connection.node as BaseNode;
        _parser = StartCoroutine(ParseNode());
    }

    public void NextGraph(){
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }
        foreach(BaseNode b in graph.nodes)
                {
                if(b.GetString() == "Start")
                {
                    graph.current = b;
                    break;
                }
        }
        _parser = StartCoroutine(ParseNode());
    }

    public void ClearChoices()
    {
        for(int i = 0; i < choiceContainer.childCount; i++)
        {
            Destroy(choiceContainer.GetChild(i).gameObject);
        }
    }


    public void ReceiveIndex(int i)
    {
        choiceIndex = i;
    }

    private void OnDestroy()
    {
        ChoiceButton.DialogueChoice -= ReceiveIndex;
    }
}
