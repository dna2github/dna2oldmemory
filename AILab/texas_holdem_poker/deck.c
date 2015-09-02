#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

typedef unsigned int uint;

// input: type = {0,1,2,3}, number = {a,2,3,4,5,6,7,8,9,o,j,q,k}
// encode: | 8b type * 100 + number |
uint card(char type, char number) {
   type -= 48;
   switch(number) {
   case 'o':
      number = 10; break;
   case 'j':
      number = 11; break;
   case 'q':
      number = 12; break;
   case 'k':
      number = 13; break;
   case 'a':
      number = 14; break;
   default:
      number -= 48;
   }
   return type * 100 + number;
}

int inline typeCount(int type) {
  return (type & 0x01)+(type>>1 & 0x01)+(type>>2 & 0x01)+(type>>3 & 0x01);
}

// encode: | 4b number | type 4b |
uint eval(uint* card) {
   uint compress[7] = {0,0,0,0,0,0,0};
   uint n = 0;

   int i, j, k, t, level, diff;

   int count_shun[5], prev_shun[5], pos_shun[5], type_same[4], pos_type[4];
   // copy
   memcpy(compress, card, sizeof(uint)*7);
   // sort
   for(i=0;i<6;i++) {
      k = i;
      for(j=i+1;j<7;j++)
         if(compress[k]%100 < compress[j]%100) k = j;
      j = compress[k]; compress[k] = compress[i]; compress[i] = j;
   }
   // compress
   j=0; k=0; compress[0] = (compress[0]%100) << 4 | (1 << (compress[0]/100));
   for(i=1;i<7;i++) {
      if((compress[k]>>4) == (compress[i]%100)) {
         compress[k] |= 1 << (compress[i] / 100);
      } else {
         k++; compress[k] = (compress[i]%100) << 4 | (1 << (compress[i]/100));
      }
   }
   for(i=k+1;i<7;i++) compress[i] = 0;
   n = k+1;
   //for(i=0;i<7;i++) printf("%x ", compress[i]); printf("\n");

   // init
   memset(count_shun, 0, 5*sizeof(uint));
   memset(prev_shun, 0, 5*sizeof(uint));
   memset(pos_shun, 0, 5*sizeof(uint));
   memset(type_same, 0, 4*sizeof(uint));
   memset(pos_type, 0, 4*sizeof(uint));

   #define LV(x) ((x) << 20)
   #define D5(x) ((x) << 16)
   #define D4(x) ((x) << 12)
   #define D3(x) ((x) << 8)
   #define D2(x) ((x) << 4)
   #define D1(x) (x)
   // level: 0=1, 1=2, 2=2+2, 3=3, 4=straight, 5=flush, 6=3+2, 7=4, 8=flush+straight
   level = 0; diff = 0;
   for(i=0;i<n;i++) {
      j = compress[i] >> 4;   // number
      k = compress[i] & 0x0f; // types

      // handle with pair, tri, quad
      //printf("card=%d, type=0x%x(%d)\n", j, k, typeCount(k));
      switch(typeCount(k)) {
      case 4:
         level = 7;
         diff  = D5(j);
         return LV(level)+diff;
         break;
      case 3:
         if(level == 1) {
            level = 6;
            diff += D5(j);
            return LV(level)+diff;
         } else if(level < 3){
            level = 3;
            diff = D5(j);
         }
         break;
      case 2:
         if(level == 3) {
            level = 6;
            diff += D4(j);
            return LV(level)+diff;
         } else if(level == 1) {
            level = 2;
            diff += D3(j);
         } else if(level == 0) {
            level = 1;
            diff += D4(j);
         }
         break;
      case 1:
         break;
      }

      // handle with straight and flush
      if(j + 1 == prev_shun[4]) {
         count_shun[4] ++;
         prev_shun[4] = j;
         if(count_shun[4] >= 5 && level < 4) {
            level = 4;
            diff = D1(compress[pos_shun[4]] >> 4);
         } else if(count_shun[4] == 4 && prev_shun[4] == 2 && compress[0] >> 4 == 14) {
            // 5 4 3 2 1
            level = 4;
            diff = D1(compress[pos_shun[4]] >> 4);
         }
      } else if(count_shun[4] < 5) {
         count_shun[4] = 1;
         prev_shun[4] = j;
         pos_shun[4] = i;
      }

      for(t=0;t<4;t++) {
         if(k & (1 << t)) {
            if(j + 1 == prev_shun[t]) {
               count_shun[t] ++;
               prev_shun[t] = j;
            } else if(count_shun[t] < 5) {
               count_shun[t] = 1;
               prev_shun[t] = j;
               pos_shun[t] = i;
            }
            if(count_shun[t] >= 5) {
               level = 8;
               diff = D1(compress[pos_shun[t]] >> 4);
               return LV(level)+diff;
            } else if(count_shun[t] == 4 && prev_shun[t] == 2 && compress[0] >> 4 == 14) {
               // 5 4 3 2 1
               level = 8;
               diff = D1(compress[pos_shun[4]] >> 4);
               return LV(level)+diff;
            }

            type_same[t] ++;
            if(pos_type[t] == 0) {
               pos_type[t] = j;
            } else if(type_same[t] >= 5 && level < 5) {
               level = 5;
               diff = D1(pos_type[t]);
            }
         } else {
            count_shun[t] = 0;
            prev_shun[t] = 0;
            pos_shun[t] = 0;
         }
      }
   } // i

   switch(level) {
   case 3: // 3++
      break;
   case 2: // 22+
      for(i=0;i<n;i++) {
         k = compress[i] & 0x0f;
         if(typeCount(k) == 1) {
            diff += D1(compress[i] >> 4);
            break;
         }
      }
      break;
   case 1: // 2+++
      t = 0;
      for(i=0;i<n;i++) {
         k = compress[i] & 0x0f;
         if(typeCount(k) == 1) {
            diff += (compress[i] >> 4) << ((2-t) * 4);
            t ++;
            if(t >= 3) break;
         }
      }
      break;
   case 0: // +++++
      diff = D5(compress[0]>>4) + D4(compress[1]>>4) + D3(compress[2]>>4) +
             D2(compress[3]>>4) + D1(compress[4]>>4);
      break;
   }

   //printf("LV=%d, DIFF=0x%x\n", level, diff);
   //printf("T=0-%d 1-%d 2-%d 3-%d\n", type_same[0], type_same[1], type_same[2], type_same[3]);
   return LV(level)+diff;
}

static const uint cardsuite[52] = {
     2,   3,   4,   5,   6,   7,   8,   9,  10,  11,  12,  13,  14,
   102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114,
   202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214,
   302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314
};

void deckShuffle(uint* cards, int reserved) {
   uint i,j,k,t;
   for(i=0;i<40;i++) {
      j = rand() % (52-reserved); j += reserved;
      k = rand() % (52-reserved); k += reserved;
      if(j == k) continue;
      t = cards[j]; cards[j] = cards[k]; cards[k] = t;
   }
}
void deckFindAndSwap(uint* cards, uint index, uint cardno) {
   uint i;
   for(i=0;i<52;i++) {
      if(cards[i] == cardno) {
         cards[i] = cards[index]; cards[index] = cardno;
         break;
      }
   }
}

int init() {
  srand(time(0));
}

int play(int n, uint* cards, int reserved) {
   uint player[7] = {0,0,0,0,0,0,0};
   uint i, t;
   deckShuffle(cards, reserved);
   memcpy(player+2, cards+2, sizeof(uint)*5);

   // player A card 0n1
   player[0] = cards[0]; player[1] = cards[1];
   t = eval(player);
   for(i=1;i<n;i++) {
      player[0] = cards[5+i*2];
      player[1] = cards[6+i*2];
      if(eval(player) > t) return 0;
   }
   return 1;
}

int main(int argc, char** argv) {
   if(argc != 4) {
      printf(
         "Usage: deck times visibleCards playerNumber\n"
         "   times: run how many times to get the frequency of winning\n"
         "   visibleCards: one card in (type, number)\n"
         "      type: 0, 1, 2, 3\n"
         "      number: 2, 3, 4, 5, 6, 7, 8, 9, o, j, q, k, a\n"
         "   playerNumber: the number of current players remaining\n"
         "e.g. deck 100000 0a1a 13\n"
         "     deck 100000 0a0k35 10\n"
      );
      exit(-1);
   }
   /*char* input = argv[1];
   uint cards[7] = {0,0,0,0,0,0,0};
   uint i, val;
   for(i=0;i<7;i++) {
      cards[i] = card(input[i*2], input[i*2+1]);
   }
   val = eval(cards);
   printf("0x%x\n", val);*/
   uint i, c;
   uint deck[52];

   char* input = argv[2];
   uint n = atoi(argv[3]),
        t = atoi(argv[1]);

   uint len = strlen(input)/2;
   if(len>7) len = 7;
   c = n*2;

   init();
   memcpy(deck, cardsuite, sizeof(uint)*52);
   switch(len) {
   case 7:
      deckFindAndSwap(deck, 6, card(input[12], input[13]));
   case 6:
      deckFindAndSwap(deck, 5, card(input[10], input[11]));
   case 5:
      deckFindAndSwap(deck, 4, card(input[8], input[9]));
   case 4:
      deckFindAndSwap(deck, 3, card(input[6], input[7]));
   case 3:
      deckFindAndSwap(deck, 2, card(input[4], input[5]));
   case 2:
      deckFindAndSwap(deck, 1, card(input[2], input[3]));
   case 1:
      deckFindAndSwap(deck, 0, card(input[0], input[1]));
      break;
   default:
      exit(1);
   }

   c = 0;
   for(i=0;i<t;i++) if(play(n,deck,len)) {/*printf("%d\n", i+1);*/c++;}
   printf("P = %.4lf\n", c*100.0/t);
   return 0;
}
