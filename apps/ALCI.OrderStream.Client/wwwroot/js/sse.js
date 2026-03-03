window.setupSSE = (dotNetHelper, fullUrl) => {
    const eventSource = new EventSource(fullUrl);

    eventSource.onopen = () => {
        console.log("SSE Stream Connected: ALCI.OrderStream.API");
        dotNetHelper.invokeMethodAsync('OnStreamStateChanged', true);
    };

    eventSource.addEventListener("order-update", (event) => {
        const data = JSON.parse(event.data);
                
        if (data.status === null) {
            console.log("SSE Stream Disconnect Signal Received:", data);
            eventSource.close();
            dotNetHelper.invokeMethodAsync('OnStreamStateChanged', false);
            return;
        }
                
        console.log(`Order Update [${data.status}]:`, data);
        dotNetHelper.invokeMethodAsync('OnOrderUpdateReceived', data);
    });

    eventSource.onerror = (error) => {        
        if (eventSource.readyState !== 2) {
            console.warn("SSE Stream Connection Closed or Finished.");
        }

        dotNetHelper.invokeMethodAsync('OnStreamStateChanged', false);
        eventSource.close();
    };

    return {
        dispose: () => {
            if (eventSource.readyState !== 2) {
                eventSource.close();
            }
        }
    };
};