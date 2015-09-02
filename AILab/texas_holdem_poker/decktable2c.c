#include <stdio.h>
#include <time.h>

#include "texgame.h"

int init(uint* deck) {
   srand(time(0));
   deckInit(deck);
}

char cardchar(uint x) {
   switch(x) {
   case 12: return 'A';
   case 11: return 'K';
   case 10: return 'Q';
   case 9:  return 'J';
   case 8:  return 'O'; // 10
   default: return (char)(x+50);
   }
}

int main(int argc, char** argv) {
   if(argc != 2) {
      printf(
         "Usage: deck times\n"
         "   times: run how many times to get the frequency of winning\n"
         "e.g. decktable2c 100000\n"
      );
      exit(-1);
   }
   uint i, j, k, p, c;
   uint deck[52];
   uint t = atoi(argv[1]);

   init(deck);
   printf("2card,2p,3p,4p,5p,6p,7p,8p,9p,10p,11p,12p,13p,14p,15p\n");

   for(j=0;j<13;j++) for(k=j+1;k<26;k++) {
      if(k%13<j%13) continue;
      printf("%d%c-%d%c",j/13,cardchar(j%13),k/13,cardchar(k%13));
      deckFindAndSwap(deck, 0, (j%13)+2+(j/13)*100);
      deckFindAndSwap(deck, 1, (k%13)+2+(k/13)*100);
      for(p=2;p<=15;p++) {
         c = 0;
         for(i=0;i<t;i++) if(play(p,deck,2)) {/*printf("%d\n", i+1);*/c++;}
         printf(",%.4lf",c*100.0/t);
         //printf("P(%d) = %.4lf\n",p,c*100.0/t);
      }
      printf("\n");
   }
   return 0;
}
