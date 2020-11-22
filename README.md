# EnvidanTaskManager
Dette projekt har til formål at undersøge brugen af Azure durable function.

Baggrunden er et konkret behov for at modernisere et større system, der står for data bearbejdelse af informationer fra forskellige dataejere og data kilder.
Ofte er der tale om mange og længerevarende processer. Der er behov for at kunne administrere de forskellige processer.
De forskellige processer skal kunne scheduleres og terminisres og man skal kunne start en bestemt proces manuelt. 
Man skal også kunne for meta information om den enkelte proces status, fremdrift og eventuelle fejl.

I projektet styres disse processer fra en asp.net core brugerflade. Der er ikke lagt vægt på en gennemført brugerflade, 
men i højere grad at underbygge at azure function kan integreres i en standardiseret løsning.

