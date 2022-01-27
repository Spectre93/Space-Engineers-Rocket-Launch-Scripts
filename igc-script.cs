string _broadCastTag = "IGC SE-RLS";
IMyBroadcastListener _myBroadcastListener;

public Program() { 
	//Register _broadCastTag to be able to receive messages
	_myBroadcastListener=IGC.RegisterBroadcastListener(_broadCastTag);
 	_myBroadcastListener.SetMessageCallback(_broadCastTag); 
}

public void Main(string argument, UpdateType updateSource){
	HandleCommunication(argument, updateSource);
}

private void HandleCommunication(string argument, UpdateType updateSource){
	// IGC code to send messages
    if ((updateSource & (UpdateType.Trigger | UpdateType.Script)) > 0){ 
        if (argument != ""){
            IGC.SendBroadcastMessage(_broadCastTag, argument);
        }
    }

	// IGC code to receive messages
    if((updateSource & UpdateType.IGC) >0){ 
        while (_myBroadcastListener.HasPendingMessage){
            MyIGCMessage myIGCMessage = _myBroadcastListener.AcceptMessage();
            if(myIGCMessage.Data is string){
                string str = myIGCMessage.Data.ToString();
                TriggerTimerFromString(str);
            }
        }
    }
}

//Triggers a timer block with the given string in the name surrounded by square brackets
private void TriggerTimerFromString(string str){
    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
    GridTerminalSystem.SearchBlocksOfName("[" + str + "]", blocks);
    try{
        var timer = blocks[0] as IMyTimerBlock;
        timer.Trigger();
    }catch(Exception e){
        Echo("'[" + str + "]' timer block missing.");
    }
}


/*
Todo
test sending tuples

var t = (speed: 4.5, altitude: 3);
Echo($"Speed is {t.speed} and rocket is currently at {t.altitude} meters up);
*/