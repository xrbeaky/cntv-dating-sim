using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeParser : MonoBehaviour
{
    [SerializeField] DialogueGraph graph;
    Coroutine _parser;

    int choiceIndex = -1;

    [SerializeField] TextMeshProUGUI speakerNameText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image speakerImage;
    [SerializeField] Transform choicePrefab;
    [SerializeField] Transform choiceContainer;
    [SerializeField] GameObject choiceParent;


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
                speakerNameText.text = dataParts[1]; //toggle which bar/sprite is visible
                dialogueText.text = dataParts[2]; //add typewriter effect
                speakerImage.sprite = b.GetSprite();

                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); 

                NextNode("exit");
                break;
            case "ChoiceNode":
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
        }
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
