// 1421A - XORwice
// we have to choose an x such that 
// a^x + b^x is minimum
//
// There are two possibilities
//  i. same bit for a and b both 1 or 0, in this case,
//       we can take a value of x which will turn of the bit for both numbers.
//  ii. same bit for a and b have different value.
// 		 this bit can not be switched off, as any value of x will reverse both or keep them same.
//  So in summary, we have to take the xor of a and b.

#include<bits/stdc++.h>
using namespace std;
#define int long long

void solve(){
	int a, b;
	cin >> a >> b;
	cout << (a^b) << "\n";
}

signed main(){
	ios_base::sync_with_stdio(0);
	cin.tie(0);cout.tie(0);
	int t; cin >> t; while(t--)
	solve();
	return 0;
}