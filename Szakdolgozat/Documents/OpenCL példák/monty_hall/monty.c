#define PROGRAM_FILE "monty.cl"
#define KERNEL_FUNC "monty"

#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#ifdef MAC
#include <OpenCL/cl.h>
#else
#include <CL/cl.h>
#endif
 
 
int main( int argc, char* argv[] )
{
    // Length of vectors
    unsigned int n = 100000;
 
    // Host input vectors
    double *car;
    double *pick;
	double *shown;
    // Host output vector
    double *stayed;
	double *changed;
    // Device input buffers
    cl_mem d_a;
    cl_mem d_b;
	cl_mem d_c;
    // Device output buffer
    cl_mem d_d;
	cl_mem d_e;
 
    cl_platform_id cpPlatform;        // OpenCL platform
    cl_device_id device_id;           // device ID
    cl_context context;               // context
    cl_command_queue queue;           // command queue
    cl_program program;               // program
    cl_kernel kernel;                 // kernel
 
    // Size, in bytes, of each vector
    size_t bytes = n*sizeof(double);
 
    // Allocate memory for each vector on host
    car = (double*)malloc(bytes);
    pick = (double*)malloc(bytes);
    shown = (double*)malloc(bytes);
	stayed = (double*)malloc(bytes);
	changed = (double*)malloc(bytes);
 
    // Initialize vectors on host
    
 
    size_t globalSize, localSize;
    cl_int err;
 
    // Number of work items in each local work group
    localSize = 64;
 
    // Number of total work items - localSize must be devisor
    globalSize = ceil(n/(float)localSize)*localSize;
 
    // Bind to platform
    err = clGetPlatformIDs(1, &cpPlatform, NULL);
 
    // Get ID for the device
    err = clGetDeviceIDs(cpPlatform, CL_DEVICE_TYPE_CPU, 1, &device_id, NULL);
 
    // Create a context  
    context = clCreateContext(0, 1, &device_id, NULL, NULL, &err);
 
    // Create a command queue 
    queue = clCreateCommandQueue(context, device_id, 0, &err);
 
    // Create the compute program from the source buffer
    program = clCreateProgramWithSource(context, 1,
                            (const char **) & kernelSource, NULL, &err);
 
    // Build the program executable 
    clBuildProgram(program, 0, NULL, NULL, NULL, NULL);
 
    // Create the compute kernel in the program we wish to run
    kernel = clCreateKernel(program, "monty", &err);
 
    // Create the input and output arrays in device memory for our calculation
    d_a = clCreateBuffer(context, CL_MEM_READ_ONLY, bytes, NULL, NULL);
    d_b = clCreateBuffer(context, CL_MEM_READ_ONLY, bytes, NULL, NULL);
    d_c = clCreateBuffer(context, CL_MEM_READ_ONLY, bytes, NULL, NULL);
	d_b = clCreateBuffer(context, CL_MEM_WRITE_ONLY, bytes, NULL, NULL);
	d_e = clCreateBuffer(context, CL_MEM_WRITE_ONLY, bytes, NULL, NULL);
 
    
 
    // Set the arguments to our compute kernel
    err  = clSetKernelArg(kernel, 0, sizeof(cl_mem), &d_a);
    err |= clSetKernelArg(kernel, 1, sizeof(cl_mem), &d_b);
    err |= clSetKernelArg(kernel, 2, sizeof(cl_mem), &d_c);
	err |= clSetKernelArg(kernel, 3, sizeof(cl_mem), &d_d);
	err |= clSetKernelArg(kernel, 4, sizeof(cl_mem), &d_e);
    err |= clSetKernelArg(kernel, 5, sizeof(unsigned int), &n);
 
    // Execute the kernel over the entire range of the data set  
    err = clEnqueueNDRangeKernel(queue, kernel, 1, NULL, &globalSize, &localSize,
                                                              0, NULL, NULL);
 
    // Wait for the command queue to get serviced before reading back results
    clFinish(queue);
 
    // Read the results from the device
    clEnqueueReadBuffer(queue, d_d, CL_TRUE, 0,
                                bytes, stayed, 0, NULL, NULL );
 
    clEnqueueReadBuffer(queue, d_e, CL_TRUE, 0,
                                bytes, changed, 0, NULL, NULL );
 
	int i;
	int changed_sum;
	int stayed_sum;
	for(i=0;i<n;i++)
	{
		stayed_sum+=stayed[i];
		changed_sum+=changed[i];
	}
	
	printf("Stayed win out of 10000: %d\n", stayed_sum);
	printf("Changed win out of 10000: %d\n", changed_sum);
	
    // release OpenCL resources
    clReleaseMemObject(d_a);
    clReleaseMemObject(d_b);
    clReleaseMemObject(d_c);
	clReleaseMemObject(d_d);
	clReleaseMemObject(d_e);
    clReleaseProgram(program);
    clReleaseKernel(kernel);
    clReleaseCommandQueue(queue);
    clReleaseContext(context);
 
    //release host memory
    free(car);
    free(changed);
    free(shown);
	free(pick);
	free(stayed);
 
    return 0;
}