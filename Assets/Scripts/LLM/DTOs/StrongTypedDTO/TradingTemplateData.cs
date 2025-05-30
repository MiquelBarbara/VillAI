using System;
using System.Collections.Generic;
using HistoryManagment;
using LLM.Templates;
using UnityEngine;

public class TradingTemplateData : DataTransferObject
{
    [Input] public int store_gold;
    [Input] public ItemContainer store_inventory;
    [Input] public int client_gold;
    [Input] public ItemContainer client_inventory;
    [Input] public String client_input;
    [Input] public List<ConversationEntry> history;
    
    
    [Output] public String shopper_response;
    [Output] public bool wantToBuy;
    [Output] public bool wantToSell;
    [Output] public String item_name;
    [Output] public Int32 amount;
}