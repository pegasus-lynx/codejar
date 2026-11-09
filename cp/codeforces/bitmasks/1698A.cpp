// 1698A - XOR Mixup
// Bitmask -  Any value can be the XOR value here

#include<bits/stdc++.h>
using namespace std;
#define int long long

void solve(){
	int n; cin >> n;
	int a[n];
	for(int i=0;i<n;i++)
	{
		cin >> a[i];
	}

	cout << a[0] << "\n";
}

signed main(){
	ios_base::sync_with_stdio(0);
	cin.tie(0);cout.tie(0);
	int t; cin >> t; while(t--)
	solve();
	return 0;
}