// OpenCL kernel. Each work item takes care of one element of c
__kernel void monty(  __global double *car,                    
                      __global double *pick,                   
                      __global double *shown,
					  __global double *stayed,
					  __global double *changed,
                       const unsigned int n)                   
{                                                             
    //Get our global thread ID                                
    int id = get_global_id(0);                                
    srand(time(NULL))                                                      
    //Make sure we do not go out of bounds                    
    if (id < n )
	{
		car[id] = rand() % 3;
		pick[id] = rand() % 3;
		do
		{
			shown[id] = rand() % 3;
		}while(shown[id] == car[id] && shown[id] == pick[id])
		
		if(pick[id] == car[id])
		{
			stayed[id]=1;
		}
		
		
		pick[id]=3-pick[id]-shown[id];
			
		if(pick[id] == car[id])
		{
			changed[id]=1;
		}
		
	
	}                                              
                                        
}                                                              
  